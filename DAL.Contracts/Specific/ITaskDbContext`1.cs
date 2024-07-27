using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DAL.Contracts
{
    public interface ITaskDbContext<out TContext> where TContext : DbContext
    {
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        TContext AsContext()
        {
            return (this as TContext) ?? throw new NullReferenceException();
        }
    }

    public interface ISetFor<T> where T : class, ITask
    {
        DbSet<T> Tasks { get; set; }
    }
    public interface ISetForMetaFile<T> where T : class, IMetaForFile
    {
        DbSet<T> Metas { get; set; }
    }

    public interface IMetaForFile
    {
        public uint MetaId { get; set; }
        public string FileDir { get; set; }
        public string FileNameOriginal { get; set; }
        public Guid ResourceId { get; set; }
        public string FileExt { get; set; }
        public uint SomeTaskId { get; set; }
        public uint LengthTotal { get; set; }
        public uint LengthUploaded { get; set; }
        public bool IsUploaded { get; set; }
    }
    public interface ITask
    {
        public uint SomeTaskId { get; set; }
        public string Name { get; set; }
        public DateTime DateTimeCreation { get; set; }
        public EStatus Status { get; set; }
    }
}
