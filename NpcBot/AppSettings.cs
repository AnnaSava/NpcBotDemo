using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot
{
    internal static class AppSettings
    {
        public static string ActiveBot { get; private set; }

        public static bool SendHealthchecks { get; private set; }

#if DEBUG
        public static int SLEEP { get { return 0; } }
#else
        public static int SLEEP { get { return 1000; } }
#endif

        public static void Read()
        {
            var config = GetConfiguration();
            ActiveBot = config["ActiveBot"];
            SendHealthchecks = bool.Parse(config["SendHealthchecks"]);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            string appsettingsPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "appsettings.json"));

            return new ConfigurationBuilder()
                .AddJsonFile(appsettingsPath, true, true)
                .Build();
        }
    }
}
