
namespace DAL.Contracts
{
    public interface IRepository12<T, TId> where T : class
    {
        ValueTask<IEnumerable<T>> GetAllAsync();
        ValueTask<T?> GetSingleAsync(TId id);
        ValueTask<T> CreateAsync(T item);
        ValueTask DeleteAsync(T item);
        ValueTask<T> DeleteAsync(TId item);
        ValueTask UpdateAsync(T item);
        bool Exists(TId id);
    }
}
