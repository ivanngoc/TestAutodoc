using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using DAL.EFCore;
using IziHardGames.ForSwagger.Attributes;
using IziHardGames.Libs.AspNetCore.AsFileServer;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using F = System.IO.File;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IDb db;
        private readonly Config config;
        public const string ContentRange = "Content-Range";

        public FilesController(IDb db, Config config)
        {
            this.db = db;
            this.config = config;
        }

        //[Authorize]
        [HttpGet("GetFile")]
        public async Task<ActionResult> GetFile(uint id)
        {
            var res = await db.Metas.FindAsync(id);
            if (res is null) return NotFound();
            string path = Path.Combine(res.FileDir, res.ResourceId.ToString());
            return File(path, "text/plain", enableRangeProcessing: true);
        }

        //[Authorize]
        [HttpGet("GetInfoForFiles")]
        public async Task<ActionResult> GetInfoForFiles()
        {
            return Ok(await db.Metas.ToListAsync());
        }
        [HttpGet("GetInfoForPhysicalFiles")]
        public async Task<ActionResult> GetInfoForPhysicalFiles()
        {
            var di = new DirectoryInfo(config.FileStoragePath);
            var files = di.GetFiles();
            return Ok(files.Select(x => new
            {
                FileName = x.Name,
                FileLength = x.Length,
                Create = x.CreationTimeUtc,
                Write = x.LastWriteTimeUtc,
            }));
        }


        /// <summary>
        /// Загрузка файла без возможности продолжить загрузку в случае прерывания
        /// </summary>
        /// <returns></returns>
        [HttpPost("V1/Upload")]
        [SwaggerFileUpload(meta: typeof(FileMeta))]
        public async Task<ActionResult> UploadWithMultipart()
        {
            var request = HttpContext.Request;

            if (!request.HasFormContentType || !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) || string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
            {
                return BadRequest("Request must be Content-UploadType of multipart");
            }

            var boundary = HeaderUtilities.RemoveQuotes(mediaTypeHeader.Boundary.Value).Value;
            var reader = new MultipartReader(boundary, request.Body);
            var section = await reader.ReadNextSectionAsync();

            var jDoc = await JsonDocument.ParseAsync(section.Body);
            Meta? meta = default;

            if (jDoc.RootElement.TryGetProperty(nameof(FileMeta.guid), out var jeGuid))
            {
                var guid = Guid.Parse(jeGuid.GetRawText().Trim('"'));
                meta = db.Metas.FirstOrDefault(x => x.ResourceId == guid);
                if (meta is null) return NotFound($"Meta with ResourceId:{guid} is not exists");
            }

            if (jDoc.RootElement.TryGetProperty(nameof(FileMeta.length), out var jeLength))
            {
                var length = jeLength.GetUInt32();

                section = await reader.ReadNextSectionAsync();

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                    if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") && contentDisposition.Name == "file" && !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                    {
                        var fileName = contentDisposition.FileName.Value;
                        var absPathToSave = Path.Combine(config.FileStoragePath, fileName);

                        if (F.Exists(absPathToSave)) F.Delete(absPathToSave);

                        using (var targetStream = System.IO.File.Create(absPathToSave))
                        {
                            await section.Body.CopyToAsync(targetStream);
                        }
                        var fi = new FileInfo(absPathToSave);
                        meta.IsUploaded = true;
                        meta.FileDir = config.FileStoragePath;
                        meta.FileExt = fi.Extension;
                        meta.FileNameOriginal = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                        meta.LengthTotal = (uint)fi.Length;
                        meta.LengthUploaded = meta.LengthTotal;
                        await db.SaveChangesAsync();
                        return Ok();
                    }
                    return BadRequest("Format error");
                }
            }
            return BadRequest("Length not specified at FileMeta boundary");
        }

        /// <summary>
        /// Загрузка файла по частям с возможностью дозаливки
        /// </summary>
        /// <returns></returns>
        [HttpPost("V2/Upload")]
        [SwaggerFileUpload(typeof(FileMeta), type: EUploadType.Range)]
        [SwaggerHeader(ContentRange, value: null, exampleValue: "bytes 0-1000/67589 ", required: true, requiredValue: false)]
        [SwaggerHeaderResponse(ContentRange, example: "bytes 0-1000/67589")]
        public async Task<ActionResult> UploadWithRange()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.ContentRange, out var contentRangeHeader))
            {
                return BadRequest("Missing Content-Range header");
            }
            var fullName = Request.Query[nameof(FileMeta.fullName)];
            var rawCreate = Request.Query[nameof(FileMeta.create)];
            var rawWrite = Request.Query[nameof(FileMeta.write)];
            var rawLength = Request.Query[nameof(FileMeta.length)];
            var rawGuid = Request.Query[nameof(FileMeta.guid)];
            var rawTaskId = Request.Query[nameof(FileMeta.taskId)];

            var guid = string.IsNullOrEmpty(rawGuid) ? default : Guid.Parse(rawGuid);
            var write = DateTime.Parse(rawWrite);
            var create = DateTime.Parse(rawCreate);
            var length = long.Parse(rawLength);
            var taskId = uint.Parse(rawTaskId);

            // TODO: some validations...

            var task = db.Tasks.Find(taskId);
            if (task is null)
            {
                return BadRequest("T is not founded");
            }

            Meta meta = default;
            if (guid != default)
            {
                meta = await db.Metas.FirstOrDefaultAsync(x => x.ResourceId == guid);
            }

            if (meta is null)
            {
                return NotFound($"Meta is not founded for given resourceId: {guid}");

                var fi = new FileInfo(fullName);
                meta = db.GetNewMeta();
                {
                    meta.ResourceId = Guid.NewGuid();
                    meta.FileDir = config.FileStoragePath;
                    meta.LengthUploaded = 0;
                    meta.FileExt = fi.Extension;
                    meta.FileNameOriginal = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
                    meta.IsUploaded = false;
                    meta.LengthTotal = (uint)length;
                    meta.SomeTask = task;
                };
                db.Metas.Add(meta);
            }
            else
            {
                if (meta.IsUploaded)
                {
                    return StatusCode(StatusCodes.Status409Conflict, $"File with this guid already uploaded, guid:{guid}");
                }
                var p = ContentRangeHeaderValue.Parse(Request.Headers.ContentRange.First());
                if (p.From != meta.LengthUploaded)
                {
                    return StatusCode(StatusCodes.Status416RequestedRangeNotSatisfiable);
                }
            }
            if (!Directory.Exists(meta.FileDir)) Directory.CreateDirectory(meta.FileDir);
            string outputPath = Path.Combine(meta.FileDir, meta.ResourceId.ToString("D"));
            using var fs = F.Open(outputPath, FileMode.OpenOrCreate);
            fs.Position = meta.LengthUploaded;
            var start = fs.Position;
            await Request.Body.CopyToAsync(fs);
            var end = fs.Position;
            var delta = (uint)(end - start);

            meta.LengthUploaded += delta;
            meta.IsUploaded = meta.LengthTotal == meta.LengthUploaded;
            await db.SaveChangesAsync();

            Response.Headers.AcceptRanges = "bytes";

            return Ok(new JsonObject()
            {
                ["consumed"] = delta,
                ["offset"] = meta.LengthUploaded,
                ["ResourceId"] = meta.ResourceId,
            });
        }
    }
}
