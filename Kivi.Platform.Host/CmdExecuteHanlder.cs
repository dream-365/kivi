using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform.Host
{
    public class CmdExecuteHanlder
    {
        public bool Handle(CmdExecuteRequest request)
        {
            var packageResolver = new PackageResolver(ConfigurationManager.AppSettings["package.server"]);

            var package = packageResolver.Resolve(request.PackageId);

            var runner = new CommandRunner(package);

            return runner.Run(request.Command, request.Arguments);
        }
    }
}
