using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Packages.CLI
{
    public class Parameters
    {
        public string OperationName { get; set; }

        public string FolderPath { get; set; }

        public static Parameters Parse(string [] args)
        {
            if (args.Length == 0)
            {
                return null;
            }

            var op = args[0];

            if (op != "pack")
            {
                Console.WriteLine("only support pack operation currently");

                return null;
            }

            if (args.Length < 2)
            {
                Console.WriteLine("please specify the folder to pack");

                return null;
            }

            var folder = args[1];

            return new Parameters
            {
                OperationName = op,
                FolderPath = folder
            };
        }
    }
}
