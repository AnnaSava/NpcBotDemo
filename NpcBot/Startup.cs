using Newtonsoft.Json;
using NpcBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot
{
    internal static class Startup
    {
        public static void Init()
        {
            AppSettings.Read();
        }
    }
}
