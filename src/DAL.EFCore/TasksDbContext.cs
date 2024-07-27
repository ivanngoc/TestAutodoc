global using IDb = DAL.Contracts.ITaskDbContext<DAL.EFCore.TasksDbContext, DAL.EFCore.SomeTask, DAL.EFCore.Meta>;
global using IDbAbstr = DAL.Contracts.ITaskDbContext<DAL.EFCore.TasksDbContext, DAL.Contracts.ITask, DAL.Contracts.IMetaForFile>;

using System.Text.Json.Serialization;
using DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.EFCore
{
    public class TasksDbContext : DbContext, IDb, IDbSet<SomeTask>, IDbSet<Meta>
    {
        public DbSet<SomeTask> Tasks { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {

        }
        public SomeTask GetNewTask()
        {
            return new SomeTask();
        }

        public Meta GetNewMeta()
        {
            return new Meta();
        }

        DbSet<SomeTask> IDbSet<SomeTask>.GetDbSet()
        {
            return Tasks;
        }
        DbSet<Meta> IDbSet<Meta>.GetDbSet()
        {
            return Metas;
        }
    }

    public class SomeTask : ITask
    {
        public uint SomeTaskId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateTimeCreation { get; set; }
        public EStatus Status { get; set; }
        public ICollection<Meta>? Files { get; set; }
    }

    [Index(nameof(ResourceId), IsUnique = true)]
    public class Meta : IMetaForFile
    {
        public uint MetaId { get; set; }
        public string FileDir { get; set; } = null!;
        public string FileNameOriginal { get; set; } = null!;
        public Guid ResourceId { get; set; }
        public string FileExt { get; set; } = null!;
        public uint SomeTaskId { get; set; }
        public uint LengthTotal { get; set; }
        public uint LengthUploaded { get; set; }
        public bool IsUploaded { get; set; }
        [JsonIgnore] public SomeTask? SomeTask { get; set; }
    }
}
