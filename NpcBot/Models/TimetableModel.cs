using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Models
{
    public class TimetableModel
    {
        public string Service { get; set; }

        public string Timetable { get; set; }

        public IEnumerable<DayModel> Dates { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Timetable);
            sb.AppendLine();

            var orderedDates = Dates.OrderBy(d => d.Date);

            foreach (var date in orderedDates)
            {
                sb.AppendLine(date.Date);

                foreach (var seance in date.Seances)
                {
                    sb.AppendLine("    " + seance);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public IEnumerable<DateTime> DatesDT
        {
            get { return Dates.Select(d => DateTime.Parse(d.Date)); }
        }
    }
}
