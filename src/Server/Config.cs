using DAL.Contracts;

namespace Server
{
    public class Config : IConfig
    {
        public const string DEF_CONN_STRING = "Host=localhost;Port=5434;Database=TestAutodoc;Username=postgres;Password=postgres";
        /// <summary>
        /// Путь, где будут сохраняться загружаемые файлы
        /// </summary>
        public string? FileStoragePath { get; set; }
        public string? ConnectionString { get; set; }
        public object this[string key]
        {
            get
            {
                switch (key)
                {
                    case nameof(FileStoragePath): return FileStoragePath;
                    case nameof(ConnectionString): return ConnectionString;
                    default: break;
                }
                throw new ArgumentOutOfRangeException();
            }

        }
    }
}
