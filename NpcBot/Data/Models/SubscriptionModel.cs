using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Models
{
    public class SubscriptionModel
    {
        public string Service { get; set; }

        public long ChatId { get; set; }

        public string Whois { get; set; }

        public string Timezone { get; set; }

        public List<string> Timetables { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
