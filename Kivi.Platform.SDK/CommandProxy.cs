using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform
{
    public class CommandProxy : MarshalByRefObject
    {
        public bool Run(Package package, string command, string arguments)
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.SetupInformation.PrivateBinPath, package.EntryAssemblyFile);

            var assembly = Assembly.LoadFile(path);

            var commandTypes = assembly.GetTypes()
                       .Where(t => t.GetInterface("ICommand") == typeof(ICommand))
                       .ToList();

            Type instanceType = null;

            foreach(var commandType in commandTypes)
            {
                var attribute = commandType.GetCustomAttribute<CommandAttribute>();

                if(attribute != null && attribute.Name.Equals(command))
                {
                    instanceType = commandType;
                    break;
                }
            }

            if(instanceType == null)
            {
                return false;
            }

            var instance = Activator.CreateInstance(instanceType) as ICommand;

            return instance.Run(arguments);
        }
    }
}
