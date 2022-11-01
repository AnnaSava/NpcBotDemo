using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Formatters
{
    internal class CalendarFormatter : ITimetableFormatter
    {
        public string Format(TimetableModel timetable)
        {
            string[] ruDays = new string[] { "вс", "пн", "вт", "🐸", "чт", "пт", "сб" };
            string[] ruDaysHeader = new string[] { "пн", "вт", "ср", "чт", "пт", "сб", "вс" };

            string[] ruMonthes = new string[] { "Январь", "Февраль", "Март",
        "Апрель", "Май", "Июнь",
        "Июль", "Август", "Сентябрь",
        "Октябрь", "Ноябрь", "Декабрь"
    };

            string[] numbers = new string[] { "0️⃣", "1️⃣", "2️⃣", "3️⃣", "4️⃣", "5️⃣", "6️⃣", "7️⃣", "8️⃣", "9️⃣", "🔟" };

            string header = string.Join(' ', ruDaysHeader.Select(d => " " + d + " "));

            var sb = new StringBuilder();


            var months = timetable.DatesDT.Select(d => new DateTime(d.Year, d.Month, 1)).Distinct();

            foreach (var month in months)
            {
                sb.AppendLine("<b>" + ruMonthes[month.Month - 1] + "</b>");
                sb.AppendLine();

                sb.Append("<code>");
                sb.AppendLine(header);

                var dayOfWeek = (int)month.DayOfWeek - 1;

                for (int i = 0; i < dayOfWeek; i++)
                {
                    sb.Append("     ");
                }

                for (int i = 1; i <= DateTime.DaysInMonth(month.Year, month.Month); i++)
                {
                    var dateModel = timetable.Dates.Where(d => d.DateDT.Year == month.Year && d.DateDT.Month == month.Month && d.DateDT.Day == i).FirstOrDefault();

                    string dayStr = dateModel != null && dateModel.Seances.Any() ? (i.ToString() + "/" + dateModel.Seances.Count() + " ") : " " + i + "  ";

                    if (i / 10 < 1)
                    {
                        sb.Append(" " + dayStr);
                    }
                    else
                    {
                        sb.Append(dayStr);
                    }

                    if ((i + dayOfWeek) % 7 == 0)
                    {
                        sb.AppendLine();
                    }
                }
                sb.Append("</code>\n");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
