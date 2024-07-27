# ENV Variables

|                   |                                            |   |
|-------------------|--------------------------------------------|---|
| FILE_STORAGE_PATH | директория для сохранения файлов           
| CONN_STRING_NAME  | имя строки подключения из appsettings.json 
|                   |                                            |   |
|                   |                                            |   |

# Запуск проекта

через
docker compose up -d

# Swagger

CLI:

dotnet tool restore

P.S. конфиги appsettings.json и/или appsettings.Development.json должны находиться в корне репозитория
dotnet swagger tofile --output "swagger.json" "src\Server\bin\Debug\net8.0\Server.dll" v1

или:
cd src\Server
dotnet swagger tofile --output "swagger.json" "bin\Debug\net8.0\Server.dll" v1

# Общее описание работы

1. создать задачу (POST /api/SomeTasks)
2. прикрепить к задаче данные о файле (POST /api/Attach)
3. загрузить файл (POST /api/Files/Upload)

# Debug
1. заполнение БД через POST /Dev/PopulateWithDummies
2. пересоздание DB через POST /Dev/RecreateDatabase