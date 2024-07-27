using System.Reflection;
using Microsoft.OpenApi.Models;
using IziHardGames.Libs.ForSwagger.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IziHardGames.Libs.ForSwagger;
public class SwaggerBigFileUploadFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.MemberInfo is MethodInfo methodInfo)
        {
            var atr = methodInfo.GetCustomAttribute<SwaggerFileUploadAttribute>();
            if(atr!=null)
            {
                //schema.bod
                //schema.
                //schema.ad
            }
        }
    }
}
