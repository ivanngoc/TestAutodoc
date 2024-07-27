using DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFCore
{
    public class Repo<T, TContext> : IRepository12<T, uint> where T : class
        where TContext : DbContext, IDbSet<T>
    {
        private readonly TContext db;
        private readonly DbSet<T> set;

        public Repo(TContext db)
        {
            this.db = db;
            this.set = db.GetDbSet();
        }
        public async ValueTask<IEnumerable<T>> GetAllAsync()
        {
            return await set.ToListAsync();
        }

        public ValueTask<T?> GetSingleAsync(uint id)
        {
            return set.FindAsync(id);
        }

        public async ValueTask<T> CreateAsync(T item)
        {
            var e = set.Add(item);
            var count = await db.SaveChangesAsync();
            return e.Entity;
        }

        public async ValueTask DeleteAsync(T item)
        {
            var e = set.Remove(item);
            await db.SaveChangesAsync();
        }

        public async ValueTask UpdateAsync(T item)
        {
            set.Update(item);
            await db.SaveChangesAsync();
        }

        public async ValueTask<T> DeleteAsync(uint id)
        {
            var res = set.Find(id);
            set.Remove(res);
            await db.SaveChangesAsync();
            return res;
        }

        public bool Exists(uint id)
        {
            return set.Find(id) != null;
        }
    }
}