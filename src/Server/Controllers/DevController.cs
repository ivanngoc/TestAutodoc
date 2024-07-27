using DAL.EFCore;
using IziHardGames.Libs.ForSwagger.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
#if DEBUG
        private readonly IDb db;
        private readonly TestSeeder testSeeder;

        public DevController(IDb db, TestSeeder testSeeder)
        {
            this.db = db;
            this.testSeeder = testSeeder;
        }
        [HttpPost(nameof(RecreateDatabase))]
        public async Task<ActionResult> RecreateDatabase([FromBody] string value)
        {
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();
            return Ok("Ensure created");
        }

        [HttpPost(nameof(EnsureTempFilesFolder))]
        public async Task<ActionResult> EnsureTempFilesFolder()
        {
            return BadRequest("NotImplemented");
        }

        [HttpPost("ExampleSwagger")]
        [ExampleIOperationFilter]
        public async Task<ActionResult> Example()
        {
            await Task.CompletedTask;
            return Ok("Some ok");
        }

        [HttpPost("PopulateWithDummies")]
        public async Task<ActionResult> Populate()
        {
            await testSeeder.PopulateWithDummies();
            return Ok();
        }
#else

#endif
    }
}
