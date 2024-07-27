using System.Reflection;
using IziHardGames.ForSwagger.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IziHardGames.Libs.ForSwagger;

/// <summary>
/// В Swagger UI появляется новый пункт с типом Header. При выполнении запроса он вставляется  
/// </summary>
public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        var atrs = context.MethodInfo.GetCustomAttributes<SwaggerHeaderAttribute>();

        foreach (var atr in atrs)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = atr.Header,
                In = ParameterLocation.Header,
                Example = new OpenApiString(atr.ExampleValue),
                Required = atr.Required,
            });
        }
    }
}