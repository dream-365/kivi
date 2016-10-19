using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kivi.Platform
{
    public class ConnectionStrings
    {
        public static string Get(string name)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[name];

            if (connectionString == null)
            {
                throw new KeyNotFoundException("does not found connection string named " + name);
            }

            return connectionString.ConnectionString;
        }
    }
}
