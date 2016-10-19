using Kivi.Platform;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace Kivi.Packages.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = Parameters.Parse(args);

            var packageFile = Path.Combine(parameters.FolderPath, "package.json");

            var txt = File.ReadAllText(packageFile);

            var package = JsonConvert.DeserializeObject<Package>(txt);

            Console.WriteLine("[Package]: {0}", package.Name);

            string achieveFileName = Guid.NewGuid().ToString() + ".zip";

            ZipFile.CreateFromDirectory(parameters.FolderPath, achieveFileName);

            using (var fs = new FileStream(achieveFileName, FileMode.Open))
            {
                var httpClient = new HttpClient();

                var multipartFormDataContent = new MultipartFormDataContent();

                var streamContent = new StreamContent(fs);

                multipartFormDataContent.Add(streamContent);

                var client = new HttpClient();

                var url = string.Format("{0}/api/packages/{1}/upload", ConfigurationManager.AppSettings["package.server"], package.Id);

                var response = httpClient.PostAsync(url, multipartFormDataContent).Result;

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    Console.WriteLine("failed to upload package, the current version [{0}] is equal or older to server side package version", package.Version);
                }
            }
        }
    }
}
