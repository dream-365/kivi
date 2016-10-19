using System;

namespace Kivi.Platform.Host
{
    public class CommandRunner
    {
        private Package _package;

        public CommandRunner(Package package)
        {
            _package = package;
        }

        private AppDomain CreateAppDomain()
        {
            var setup = new AppDomainSetup()
            {
                ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                LoaderOptimization = LoaderOptimization.MultiDomainHost,
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                PrivateBinPath = string.Format(@"Packages\{0}.{1}", _package.Id, _package.Version) ,
                PrivateBinPathProbe = ""
            };

            return AppDomain.CreateDomain(
                Guid.NewGuid().ToString(),
                AppDomain.CurrentDomain.Evidence,
                setup);
        }

        public bool Run(string name, string arguments)
        {
            var domain = CreateAppDomain();

            var proxy = (CommandProxy)domain.CreateInstanceAndUnwrap(
                        typeof(CommandProxy).Assembly.FullName,
                        typeof(CommandProxy).FullName);

            bool result;

            try
            {
                result = proxy.Run(_package, name, arguments);
            }
            catch (Exception)
            {
                result = false;
            }

            AppDomain.Unload(domain);

            return result;
        }
    }
}
