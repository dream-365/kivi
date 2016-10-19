using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform
{
    public interface ICommand
    {
        bool Run(string arguments);
    }
}
