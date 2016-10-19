using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }

        public string Arguments { get; set; }

        public string Description { get; set; }
    }
}
