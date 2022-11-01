using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Models
{
    public class DayModel
    {
        public string Doctor { get; set; }

        public string Date { get; set; }

        public IEnumerable<string> Seances { get; set; }

        public DateTime DateDT
        {
            get { return DateTime.Parse(Date); }
        }

        public IEnumerable<DateTime> SeancesDT
        {
            get { return Seances.Select(s => DateTime.Parse(s)); }
        }
    }
}
