using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace IziHardGames.Libs.ForSwagger
{
    public static class ExtensionsForBuilderAsSwaggerLib
    {
        public static void AddIziSwagger(this IServiceCollection services, IziSwaggerConfig config)
        {
            throw new System.NotImplementedException();
        }
        public static void AddIziSwagger(this IServiceCollection services, Assembly assembly)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

                var dic = new Dictionary<Assembly, XPathDocument>();
                var asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var item in asms)
                {
                    if (item == assembly) continue;
                    var xmlDoc = $"{item.GetName().Name}.xml";
                    var pathAbs = Path.Combine(AppContext.BaseDirectory, xmlDoc);
                    if (File.Exists(pathAbs))
                    {
                        var xpd = new XPathDocument(pathAbs);
                        options.IncludeXmlComments(() => xpd, true);
                        dic.Add(item, xpd);
                    }
                }

                options.EnableAnnotations();
                var xmlFilename = $"{assembly.GetName().Name}.xml";
                string path = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (!File.Exists(path)) throw new FileNotFoundException(path);
                var xmlDox = new XPathDocument(path);
                options.IncludeXmlComments(() => xmlDox, true);
                dic.Add(assembly, xmlDox);

                XmlRepo repo = new XmlRepo(dic);

                options.OperationFilter<SwaggerQueryStringOperationFilter>();
                options.OperationFilter<ExampleIOperationFilter>();
                options.OperationFilter<AddRequiredHeaderParameter>();
                options.SchemaFilter<SwaggerTryItOutDefaultValueFilter>();
                options.OperationFilter<FormFileSwaggerFilter>(repo);
            });
        }
    }
}
