using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.HttpClients.Calendly
{
    internal class Spot
    {
        public string Status { get; set; }

        public string StartTime { get; set; }

        public int InviteesRemaining { get; set; }
    }
}
