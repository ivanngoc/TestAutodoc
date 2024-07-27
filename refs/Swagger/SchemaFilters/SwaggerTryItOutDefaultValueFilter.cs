using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using IziHardGames.Libs.ForSwagger.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IziHardGames.Libs.ForSwagger;
public class SwaggerTryItOutDefaultValueFilter : ISchemaFilter
{
    /// <summary>
    /// Apply is called for each parameter
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.ParameterInfo != null)
        {
            var att = context.ParameterInfo.GetCustomAttribute<SwaggerTryItOutDefaultValueAttribute>();
            if (att != null)
            {
                var type = att.Value.GetType();
                if (type == typeof(string))
                {
                    schema.Example = new OpenApiString((string)att.Value);
                }
                else if (type == typeof(int))
                {
                    schema.Example = new OpenApiInteger((int)att.Value);
                }
                else
                {
                    throw new NotImplementedException(type.AssemblyQualifiedName);
                }
            }
        }
    }
}