using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Reflection;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IziHardGames.Libs.ForSwagger;

public class ExampleIOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var atr = context.MethodInfo.GetCustomAttribute<ExampleIOperationFilterAttribute>();
        if (atr != null)
        {
            if (operation.RequestBody is null)
            {
                operation.RequestBody = new OpenApiRequestBody()
                {

                };
            }

            OpenApiSchema schema = new OpenApiSchema
            {
                Type = "object",
                Description = "file's fullname include extension",
                Example = null,
                AdditionalProperties = atr.Model0 is null ? null : context.SchemaGenerator.GenerateSchema(atr.Model0, context.SchemaRepository)
            };

            operation.RequestBody.Content.Add("multipart/form-data", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["complexSchema"] = schema,
                        // ключ это ContentDisposition
                        ["filename"] = new OpenApiSchema
                        {
                            //UploadType = "string",
                            Format = "string",
                            Description = "file's fullname include extension",
                            Example = new OpenApiString("somedocument.txt"),
                        },
                        ["dateDime"] = new OpenApiSchema
                        {
                            //UploadType = "date-time",
                            Format = "date-time",
                            Description = "some date time",
                            Example = new OpenApiDateTime(DateTimeOffset.Now)
                        },
                        ["file"] = new OpenApiSchema { Type = "string", Format = "binary" },
                        ["random"] = new OpenApiSchema
                        {
                            //UploadType = "string",
                            Format = "ipv6",
                            //Example = new
                        },
                        ["myString"] = new OpenApiSchema
                        {
                            //UploadType = "string",
                            Example = new OpenApiString("This is string value"),
                        },
                        //["string"] = new OpenApiSchema { UploadType = "string", Format = "utf8" },
                        //["application/json"] = new OpenApiMediaType()
                        //{
                        //    Schema = context.SchemaGenerator.GenerateSchema(typeof(BadRequest), context.SchemaRepository),
                        //}
                    }
                },
                // если schema создана, то каждое поле в типе atr.Meta будет отдельным boundry
                Encoding = new Dictionary<string, OpenApiEncoding>()
                {
                    ["file"] = new OpenApiEncoding
                    {
                        ContentType = "application/octet-stream"
                    },
                    ["filemeta"] = new OpenApiEncoding
                    {
                        ContentType = "application/json",
                        Style = ParameterStyle.Form
                    },
                    ["myString"] = new OpenApiEncoding
                    {
                        ContentType = "text/plain",
                        Style = ParameterStyle.Form
                    },
                    ["complexSchema"] = new OpenApiEncoding
                    {
                        ContentType = "application/json",
                        //Style = ParameterStyle.DeepObject
                    }
                },
            });

            operation.RequestBody.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["file"] = new OpenApiSchema { Type = "string", Format = "binary" },
                        //["string"] = new OpenApiSchema { UploadType = "string", Format = "utf8" },
                        //["application/json"] = new OpenApiMediaType()
                        //{
                        //    Schema = context.SchemaGenerator.GenerateSchema(typeof(BadRequest), context.SchemaRepository),
                        //}
                    }
                }
            });


            foreach (var requestContent in operation.RequestBody.Content)
            {
                var encodings = requestContent.Value.Encoding;
                foreach (var encoding in encodings)
                {
                    Console.WriteLine($"{requestContent.Key}:{encoding}");
                    //encoding.Value.ContentType = "application/x-izi-type-v1";
                }

                //foreach (var item in requestContent.Value.)
                {

                }
            }

        }
    }
}
