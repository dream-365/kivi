using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandExeRquest = new CmdExecuteRequest
            {
                PackageId = "CA.Data.Sync",
                Command = "user-voice-sync",
                Arguments = null
            };

            var handler = new CmdExecuteHanlder();

            var result = handler.Handle(commandExeRquest);
        }
    }
}
