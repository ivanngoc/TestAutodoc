
using System.Reflection;
using DAL.Contracts;
using DAL.EFCore;
using IziHardGames.Libs.ForSwagger;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using static Server.ConstantsForTestAutodoc;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Console.WriteLine(builder.Configuration.ToJson(Newtonsoft.Json.Formatting.Indented));
            var name = Environment.GetEnvironmentVariable("CONN_STRING_NAME") ?? throw new NullReferenceException("Env Var CONN_STRING_NAME is not defined"); 
            var connstring = builder.Configuration.GetConnectionString(name);
            Console.WriteLine($"CONN_STRING: {connstring}");
            if (string.IsNullOrEmpty(connstring)) throw new NullReferenceException($"Connection string at appsettings.json is not defined. {Directory.GetCurrentDirectory()}");
            Config config = new Config()
            {
                FileStoragePath = Environment.GetEnvironmentVariable(ENV_VAR_FILE_STORAGE_PATH) ?? throw new NullReferenceException($"Environemnt variable:{ENV_VAR_FILE_STORAGE_PATH} is not defined"),
                ConnectionString = connstring,
            };

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBoundaryLengthLimit = (1 << 20) * 4;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
            // Add services to the container.
#if DEBUG
            builder.Services.AddScoped<TestSeeder>();
#endif
            builder.Services.AddScoped<IRepository12<Meta, uint>, Repo<Meta, TasksDbContext>>();
            builder.Services.AddSingleton(config);
            builder.Services.AddSingleton<IConfig, Config>((x) => config);
            builder.Services.AddControllers();

            builder.Services.AddDbContextPool<IDb, TasksDbContext>(b => b.UseNpgsql(config.ConnectionString));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddIziSwagger(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(c => c.InjectJavascript("/swagger-ui/custom.js"));
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<IDb>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

            app.UseAuthorization();

            app.MapControllers();
            app.MapGet("", () => Results.Ok("Probed"));

            app.Run();
        }
    }
}
