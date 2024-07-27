using System.Reflection;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IziHardGames.Libs.ForSwagger;

public class SwaggerQueryStringOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var atr = context.MethodInfo.GetCustomAttribute<SwaggerQueryStringAttribute>();
        if (atr != null)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "FileMetaQuery",
                In = ParameterLocation.Query,
                Example = new OpenApiString(atr.Example),
                //context.SchemaGenerator.GenerateSchema(atr.UploadType, context.SchemaRepository),
            });
        }
    }
}
