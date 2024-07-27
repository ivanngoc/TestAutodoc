namespace IziHardGames.ForSwagger.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class SwaggerHeaderResponseAttribute : Attribute
{
    public string HeaderName { get; }
    public object? Example { get; }

    public SwaggerHeaderResponseAttribute(string name, object? example = null)
    {
        this.HeaderName = name;
        this.Example = example;
    }
}
