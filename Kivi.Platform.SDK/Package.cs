using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform
{

    [Serializable]
    public class Package
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EntryAssemblyFile { get; set; }

        public string Version { get; set; }
    }
}
