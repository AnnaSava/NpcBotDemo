using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.HttpClients.Calendly
{
    internal class TimeTable
    {
        public bool InviteePublisherError { get; set; }

        public string Today { get; set; }

        public string AvailabilityTimezone { get; set; }

        public List<Day> Days { get; set; }
    }
}
