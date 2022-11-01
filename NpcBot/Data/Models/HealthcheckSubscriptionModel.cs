using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Models
{
    public class HealthcheckSubscriptionModel
    {
        public long ChatId { get; set; }

        public int TimeUtc { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
