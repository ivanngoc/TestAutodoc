using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Swashbuckle.AspNetCore.SwaggerGen.XmlApplyHelper;

namespace IziHardGames.Libs.ForSwagger;

public class FormFileSwaggerFilter : IOperationFilter
{
    private readonly XmlRepo repo;
    public FormFileSwaggerFilter(XmlRepo repo)
    {
        this.repo = repo;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var atr = context.MethodInfo.GetCustomAttribute<SwaggerFileUploadAttribute>();
        if (atr != null)
        {
            if (operation.RequestBody is null)
            {
                operation.RequestBody = new OpenApiRequestBody()
                {

                };
            }

            if (atr.UploadType == EUploadType.Multipart)
            {
                OpenApiSchema schema = atr.Meta is null ? new OpenApiSchema()
                {
                    Type = "object",
                    Description = "file's fullname include extension",
                    Example = null,
                    //AdditionalProperties = atr.Meta is null ? null :
                } : context.SchemaGenerator.GenerateSchema(atr.Meta, context.SchemaRepository);

                operation.RequestBody.Content.Add("multipart/form-data", new OpenApiMediaType
                {
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
                        }
                    },
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            // ключ это ContentDisposition
                            ["filemeta"] = schema,
                            ["file"] = new OpenApiSchema
                            {
                                Type = "file",
                                Format = "formData",
                            },
                        }
                    }
                });
            }
            else if (atr.UploadType == EUploadType.Range)
            {
                operation.RequestBody.Content.Add("application/octet-stream", new OpenApiMediaType
                {
                    // если schema создана, то каждое поле в типе atr.Meta будет отдельным boundry
                    Encoding = new Dictionary<string, OpenApiEncoding>()
                    {
                        ["file"] = new OpenApiEncoding
                        {
                            ContentType = "application/octet-stream"
                        },
                    },
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["file"] = new OpenApiSchema
                            {
                                Type = "file",
                                Format = "formData",
                            },
                        }
                    }
                });

                if (atr.Meta != null)
                {
                    foreach (var prop in atr.Meta.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var val = prop.GetCustomAttribute<DefaultValueAttribute>()?.Value?.ToString();
                        var oApiParam = new OpenApiParameter()
                        {
                            Name = prop.Name,
                            In = ParameterLocation.Query,
                            Required = prop.GetCustomAttribute<RequiredAttribute>() != null,
                            Schema = context.SchemaGenerator.GenerateSchema(prop.PropertyType, context.SchemaRepository, memberInfo: prop),
                            Example = val is null ? new OpenApiString("Replaced") : new OpenApiString(val),
                        };
                        //oApiParam.Schema.Xml = default;
                        operation.Parameters.Add(oApiParam);
                    }
                }


                var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor == null) return;

                var commentId = XmlCommentsIdHelper.GetCommentIdForMethod(controllerActionDescriptor.MethodInfo);
                var methodNode = repo[atr.Meta].SelectSingleNode(string.Format(MemberXPath, commentId));

                ApplyParametersExamples(atr.Meta, operation.Parameters, context.ApiDescription, repo);

                foreach (var requestContent in operation.RequestBody.Content)
                {
                    var encodings = requestContent.Value.Encoding;
                    foreach (var encoding in encodings)
                    {
                        //encoding.Value.ContentType = value;
                    }
                }
            }
        }
    }

}