using Kivi.Platform;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;

namespace Kivi.Packages.Server
{
    public class PackageHelper
    {
        public static bool IsNewer(string currentVersion, string newVersion)
        {
            var ver1 = Version.Parse(currentVersion);
            var ver2 = Version.Parse(newVersion);

            return ver2 > ver1;
        }

        public static Package ReadPackageFromZipStream(Stream stream)
        {
            Package package;
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                var entry = zip.GetEntry("package.json");

                using (var packageFileStream = entry.Open())
                using (var streamReader = new StreamReader(packageFileStream))
                {
                    package = JsonConvert.DeserializeObject<Package>(streamReader.ReadToEnd());
                }
            }

            return package;
        }
    }
}