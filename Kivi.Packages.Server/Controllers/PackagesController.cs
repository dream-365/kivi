using Kivi.Packages.Server.Models;
using Kivi.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kivi.Packages.Server.Controllers
{
    public class PackagesController : ApiController
    {
        private PackagesDbContext _context;

        public PackagesController()
        {
            _context = new PackagesDbContext();
        }

        [Route("api/packages")]
        [HttpGet]
        public IEnumerable<PackageModel> Get()
        {
            return _context.Packages.ToList();
        }

        [Route("api/packages/{id}")]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            var package = _context.Packages.Find(id);

            if(package == null)
            {
                return NotFound();
            }else
            {
                return Json(package);
            }
        }

        [Route("api/packages/{id}/download")]
        [HttpGet]
        public HttpResponseMessage Download(string id)
        {
            var package = _context.Packages.Find(id);

            if(package == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            var packageFolderPath = HttpContext.Current.Server.MapPath("~/App_Data");

            var packageFilePath = Path.Combine(packageFolderPath, package.Id + "." + package.Version + ".zip");

            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(packageFilePath, FileMode.Open))
            {
                fs.CopyTo(memoryStream);
                memoryStream.Position = 0;
            }

            result.Content = new StreamContent(memoryStream);

            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = package.Id + "." + package.Version + ".zip"
                };

            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        [Route("api/packages/{id}/upload")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload(string id)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync();

            var content = provider.Contents.FirstOrDefault();
            var uploadFolderPath = HttpContext.Current.Server.MapPath("~/App_Data");
            

            using (var stream = await content.ReadAsStreamAsync())
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);

                Package package = PackageHelper.ReadPackageFromZipStream(stream);

                PackageModel currentPackage = _context.Packages.Find(package.Id);

                if(currentPackage != null)
                {
                    var newVersion = Version.Parse(package.Version);
                    var currentVersion = Version.Parse(currentPackage.Version);

                    if(currentVersion >= newVersion)
                    {
                        return StatusCode(HttpStatusCode.Conflict);
                    }

                    currentPackage.Version = package.Version;
                }
                else{
                    _context.Packages.Add(new PackageModel {
                        Id = package.Id,
                        Name = package.Name,
                        Version = package.Version,
                        EntryAssemblyFile = package.EntryAssemblyFile
                    });
                }

                _context.SaveChanges();

                memoryStream.Position = 0;

                var uploadFilePath = Path.Combine(uploadFolderPath, package.Id + "." + package.Version + ".zip");

                using (var fs = new FileStream(uploadFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    memoryStream.CopyTo(fs);
                }

                return Json(package);
            }
        }
    }
}
