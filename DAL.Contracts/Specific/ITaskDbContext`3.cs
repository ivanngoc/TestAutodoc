using Microsoft.EntityFrameworkCore;

namespace DAL.Contracts
{
    public interface ITaskDbContext<out TContext, T1, T2> : ISetFor<T1>, ISetForMetaFile<T2>, ITaskDbContext<TContext>
        where TContext : DbContext
        where T1 : class, ITask
        where T2 : class, IMetaForFile
    {
        T1 GetNewTask();
        T2 GetNewMeta();
    }
}
