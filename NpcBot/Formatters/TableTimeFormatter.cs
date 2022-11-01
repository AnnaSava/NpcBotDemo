using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Formatters
{
    internal class TableTimeFormatter : ITimetableFormatter
    {
        public string Format(TimetableModel timetable)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<b>" + timetable.Timetable + "</b>");
            sb.AppendLine();

            var orderedDates = timetable.Dates.OrderBy(d => d.Date);

            foreach (var date in orderedDates)
            {
                sb.AppendLine(date.Date);

                sb.Append(" ");

                int i = 0;
                foreach (var seance in date.Seances)
                {
                    i++;
                    sb.Append(seance.Substring(0, seance.LastIndexOf(':')));
                    if (i % 5 == 0)
                    {
                        sb.AppendLine();
                        sb.Append(" ");
                    }
                    else
                        sb.Append("   ");

                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
