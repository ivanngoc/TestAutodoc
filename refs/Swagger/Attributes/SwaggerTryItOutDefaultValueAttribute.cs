namespace IziHardGames.Libs.ForSwagger.Attributes;

public class SwaggerTryItOutDefaultValueAttribute : Attribute
{
    public object Value { get; }

    public SwaggerTryItOutDefaultValueAttribute(object value)
    {
        Value = value;
    }
}