using Newtonsoft.Json.Serialization;
using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.HttpClients.Calendly
{
    public class CalendlyClient : BaseClient
    {
        const string API = "https://calendly.com/api/booking/event_types/";

        const string QueryTemplate = "{GUID}/calendar/range?timezone=Europe/Moscow&diagnostics=false&range_start={0}&range_end={1}";

        public CalendlyClient() : base(API, new SnakeCaseNamingStrategy()) { }

        public override async Task<IEnumerable<TimetableModel>> GetUpdates(IEnumerable<string> timetablesToCheck)
        {
            var timetable = await GetTimeTable(DateTime.Now);

            var doctor = new TimetableModel
            {
                Service = "Calendly",
                Timetable = "GUID"
            };

            doctor.Dates = timetable.Days.Select(m => new DayModel
            {
                Date = m.Date,
                Seances = m.Spots.Select(m => m.StartTime.ToString())
            });

            return new List<TimetableModel> { doctor };
        }

        private async Task<TimeTable> GetTimeTable(DateTime dateStart)
        {
            var query = string.Format(QueryTemplate, dateStart.ToString("yyyy-MM-dd"), dateStart.AddMonths(1).ToString("yyyy-MM-dd"));
            var timetable = await SendGetRequest<TimeTable>(query);
            return timetable;
        }
    }
}
