namespace IziHardGames.Libs.ForSwagger.Attributes;

public class TitleAttribute : Attribute
{
    private readonly string value;

    public TitleAttribute(string value)
    {
        this.value = value;
    }   
}