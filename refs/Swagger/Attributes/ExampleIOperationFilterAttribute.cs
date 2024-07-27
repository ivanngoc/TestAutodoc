using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace IziHardGames.Libs.ForSwagger.Attributes;

/// <summary>
/// Execute custom code. Потенциальная угроза
/// </summary>
public class CodeEvalAttribute : Attribute
{

}

public class ExampleIOperationFilterAttribute : Attribute
{
    public Type? Model0 { get; }
    public Type? Model1 { get; }
    public Type? Model2 { get; }

    public ExampleIOperationFilterAttribute(Type? model0 = default, Type? model1 = default, Type? model2 = default) : base()
    {
        this.Model0 = model0 ?? typeof(Model0);
        this.Model1 = model1 ?? typeof(Model1);
        this.Model2 = model2 ?? typeof(Model2);
    }
}

internal class Model0(int ValAsInt, float ValAsFloat, long ValAsLong, DateTime ValAsDateTime) { }
internal class Model1()
{
    [EmailAddress] public string ValAsString { get; set; }
}
internal class Model2(JsonDocument jDoc, JsonObject jObj) { }

