using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.HttpClients.Calendly
{
    internal class Day
    {
        public string Date { get; set; }

        public string Status { get; set; }

        public List<Spot> Spots { get; set; }
    }
}
