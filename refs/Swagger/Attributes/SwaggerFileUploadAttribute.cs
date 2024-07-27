namespace IziHardGames.Libs.ForSwagger.Attributes;
public class SwaggerFileUploadAttribute : Attribute
{
    public Type? Meta { get; private set; }
    public EUploadType UploadType { get; }

    public SwaggerFileUploadAttribute(Type? meta = null, EUploadType type = EUploadType.Multipart)
    {
        this.Meta = meta;
        this.UploadType = type;
    }
}
