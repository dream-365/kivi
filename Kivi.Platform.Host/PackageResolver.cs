using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform.Host
{
    public class PackageResolver
    {
        static PackageResolver()
        {
            if (!Directory.Exists("Packages"))
            {
                Directory.CreateDirectory("Packages");
            }
        }

        private string _serverAddress;

        public PackageResolver(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public Package Resolve(string packageId)
        {
            var client = new HttpClient();

            var text = client
                        .GetStringAsync(string.Format("{0}/api/packages/{1}", _serverAddress, packageId))
                        .Result;

            var package = Newtonsoft.Json.JsonConvert.DeserializeObject<Package>(text);

            var folderName = Path.Combine("Packages", string.Format("{0}.{1}", package.Id, package.Version));

            if(!Directory.Exists(folderName))
            {
                var stream = client.GetStreamAsync(string.Format("{0}/api/packages/{1}/download", _serverAddress, packageId))
                                   .Result;

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);

                    memoryStream.Position = 0;

                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        zip.ExtractToDirectory(folderName);
                    }
                }

                stream.Close();
            }

            return package;
        }
    }
}
