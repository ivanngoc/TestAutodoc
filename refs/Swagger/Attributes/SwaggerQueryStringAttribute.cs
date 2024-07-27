namespace IziHardGames.Libs.ForSwagger.Attributes;

public class SwaggerQueryStringAttribute : Attribute
{
    public Type? Type { get; }
    public string? Example { get; }

    public SwaggerQueryStringAttribute(Type? type = default, string? example = null)
    {
        this.Type = type;
        this.Example = example;
    }
}
