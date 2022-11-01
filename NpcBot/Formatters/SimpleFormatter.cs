using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Formatters
{
    internal class SimpleFormatter : ITimetableFormatter
    {
        public string Format(TimetableModel timetable)
        {
            var sb = new StringBuilder();

            sb.AppendLine(timetable.Timetable);
            sb.AppendLine();

            var orderedDates = timetable.Dates.OrderBy(d => d.Date);

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
    }
}
