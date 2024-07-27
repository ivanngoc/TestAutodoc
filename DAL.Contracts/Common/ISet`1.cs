using Microsoft.EntityFrameworkCore;

namespace DAL.Contracts
{
    public interface IDbSet<T> where T : class
    {
        DbSet<T> GetDbSet();
    }
}
