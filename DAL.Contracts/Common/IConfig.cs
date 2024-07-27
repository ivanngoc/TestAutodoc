namespace DAL.Contracts
{
    public interface IConfig
    {
        object this[string key] { get; }
    }
}
