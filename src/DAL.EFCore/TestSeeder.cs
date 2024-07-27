
using DAL.Contracts;

namespace DAL.EFCore
{
    public class TestSeeder
    {
        private readonly IDb db;
        private readonly IConfig config;
        private static readonly string[] guids = new[]
        {
            "ca31f0a3-2f70-4d7e-b543-22af5ff8c4a4",
            "3f6f8a44-7e45-4b50-ba6f-2517ea9e0b95",
            "c76fa396-3fca-47a9-9890-39491b222cee",
            "71839178-fd4a-462a-aac3-3740ba0c0f18",
            "76692eef-e0fa-40aa-afbb-30108330745e",
            "363f7fad-07d0-46f0-8506-cdca2d0c6d95",
            "2703d9e7-4ff4-4391-b32d-1c9420ab7676",
            "4680b757-70f4-4a46-8f48-ba8ce99735d6",
        };

        public TestSeeder(IDb db, IConfig config)
        {
            this.db = db;
            this.config = config;
        }

        public async Task PopulateWithDummies()
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            var arr = new Meta[guids.Length];
            SomeTask someTask = new SomeTask()
            {
                SomeTaskId = default,
                Files = arr,
                Name = "Test",
                Status = EStatus.Created,
            };
            for (uint i = 0; i < guids.Length; i++)
            {
                arr[i] = new Meta()
                {
                    ResourceId = Guid.Parse(guids[i]),
                    FileNameOriginal = $"SomeFile_{i}",
                    FileExt = ".txt",
                    LengthTotal = 100 + i,
                    IsUploaded = false,
                    LengthUploaded = 0,
                    FileDir = config["FileStoragePath"].ToString(),
                    MetaId = default,
                    SomeTask = default,
                    SomeTaskId = default,
                };
            }
            db.Tasks.Add(someTask);
            await db.SaveChangesAsync();
        }
    }
}