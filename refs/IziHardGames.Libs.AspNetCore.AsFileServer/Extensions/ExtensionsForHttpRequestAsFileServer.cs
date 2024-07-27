using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IziHardGames.Libs.AspNetCore.AsFileServer.Extensions
{
    public static class ExtensionsForHttpRequestAsFileServer
    {
        public static async ValueTask<ActionResult> UploadFileWithMultipart(this HttpContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
