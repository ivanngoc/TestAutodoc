using IziHardGames.Libs.ForSwagger;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Xml.XPath;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public static class XmlApplyHelper
    {
        public const string MemberXPath = "/doc/members/member[@name='{0}']";
        public const string SummaryXPath = "summary";
        public const string RemarksXPath = "remarks";
        public const string ExamplesXPath = "example";
        public const string CodeXPath = "code";
        public const string ParamXPath = "param[@name='{0}']";
        public const string ResponsesXPath = "response";

        public static void ApplyMethodXmlToOperation(OpenApiOperation operation, XPathNavigator methodNode, XPathNavigator xmlNavigator)
        {
            var summaryNode = methodNode.SelectSingleNode(SummaryXPath);
            if (summaryNode != null)
                operation.Summary = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);

            var remarksNode = methodNode.SelectSingleNode(RemarksXPath);
            if (remarksNode != null)
                operation.Description = XmlCommentsTextHelper.Humanize(remarksNode.InnerXml);
        }

        public static void ApplyParamsXmlToActionParameters(
            IList<OpenApiParameter> parameters,
            XPathNavigator methodNode,
            ApiDescription apiDescription, XPathNavigator xmlNavigator)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                // Check for a corresponding action parameter?
                var actionParameter = apiDescription.ActionDescriptor.Parameters
                    .FirstOrDefault(p => parameter.Name.Equals((p.BindingInfo?.BinderModelName ?? p.Name), StringComparison.OrdinalIgnoreCase));

                if (actionParameter == null) continue;

                var paramNode = methodNode.SelectSingleNode(string.Format(ParamXPath, actionParameter.Name));

                if (paramNode != null)
                    parameter.Description = XmlCommentsTextHelper.Humanize(paramNode.InnerXml);
            }
        }

        public static void ApplyResponsesXmlToResponses(OpenApiResponses responses, XPathNodeIterator responseNodes, XPathNavigator xmlNavigator)
        {
            while (responseNodes.MoveNext())
            {
                var code = responseNodes.Current.GetAttribute("code", "");
                var response = responses.ContainsKey(code) ? responses[code] : responses[code] = new OpenApiResponse();
                response.Description = XmlCommentsTextHelper.Humanize(responseNodes.Current.InnerXml);
            }
        }

        public static void ApplyPropertiesXmlToPropertyParameters(
            IList<OpenApiParameter> parameters,
            ApiDescription apiDescription,
            XPathNavigator xmlNavigator)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                // Check for a corresponding  API parameter (from ApiExplorer) that's property-bound?
                var propertyParam = apiDescription.ParameterDescriptions
                    .Where(p => p.ModelMetadata?.ContainerType != null && p.ModelMetadata?.PropertyName != null)
                    .FirstOrDefault(p => parameter.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyParam == null) continue;

                var metadata = propertyParam.ModelMetadata;
                var propertyInfo = metadata.ContainerType.GetTypeInfo().GetProperty(metadata.PropertyName);
                if (propertyInfo == null) continue;

                var commentId = XmlCommentsIdHelper.GetCommentIdForProperty(propertyInfo);
                var propertyNode = xmlNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));
                if (propertyNode == null) continue;

                var summaryNode = propertyNode.SelectSingleNode(SummaryXPath);
                if (summaryNode != null)
                    parameter.Description = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
            }
        }

        [CodeEval]
        public static void ApplyParametersExamples(Type meta, IList<OpenApiParameter> parameters, ApiDescription apiDescription, XmlRepo repo)
        {
            var xmlNavigator = repo[meta];

            foreach (var oApiParam in parameters)
            {
                if (oApiParam.Schema != null)
                {
                    var type = oApiParam.Schema.Type;
                    var prop = meta.GetProperty(oApiParam.Name);
                    if (prop != null) { } // must exist?
                    var commentId = XmlCommentsIdHelper.GetCommentIdForProperty(prop);
                    var propertyNode = xmlNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));
                    if (propertyNode != null)
                    {
                        var node = propertyNode.SelectSingleNode(ExamplesXPath);
                        if (node != null && !string.IsNullOrEmpty(node.InnerXml))
                        {
                            var nodeCode = node.SelectSingleNode(CodeXPath);

                            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<DateTime>))
                            {
                                oApiParam.Example = new OpenApiDateTime(DateTime.Now);
                            }
                            else
                            {

                                if (nodeCode != null)
                                {
                                    // eval here
                                }
                                else
                                {
                                    oApiParam.Example = new OpenApiString(node.InnerXml);
                                }
                            }
                        }
                    }
                    //if(oApiParam.In == ParameterLocation.Query)
                }
            }
        }

        public static void ApplyXmlCustomComments(PropertyInfo prop, OpenApiSchema apiSchema, XPathNavigator xmlNavigator)
        {
            throw new System.NotImplementedException();
        }
    }
}