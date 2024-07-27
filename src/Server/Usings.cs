global using IDb = DAL.Contracts.ITaskDbContext<DAL.EFCore.TasksDbContext, DAL.EFCore.SomeTask, DAL.EFCore.Meta>;
global using IDbAbst = DAL.Contracts.ITaskDbContext<DAL.EFCore.TasksDbContext, DAL.Contracts.ITask, DAL.Contracts.IMetaForFile>;



