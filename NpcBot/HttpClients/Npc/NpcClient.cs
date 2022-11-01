using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NpcBot.Data;
using NpcBot.HttpClients;
using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot
{
    public class NpcClient : BaseClient
    {
        const string NPC_API = "APIURL";
        readonly DataContext dataContext;

        public NpcClient(DataContext dataContext) : base(NPC_API, new CamelCaseNamingStrategy())
        {
            this.dataContext = dataContext;
        }

        public override async Task<IEnumerable<TimetableModel>> GetUpdates(IEnumerable<string> timetablesToCheck)
        {
            var foundDates = new List<DayModel>();

            var clinicsToCheck = GetClinicsAndDoctorsToCheck(timetablesToCheck);
            foreach (var clinic in clinicsToCheck)
            {
                var dates = await GetDates(clinic.ClinicId);
                if (dates == null || !dates.Any()) continue;

                foreach (var date in dates.ToList().OrderBy(d => d))
                {
                    var doctors = await GetDoctors(clinic.ClinicId, date);
                    if (doctors == null) continue;

                    foreach (var doctorToCheck in timetablesToCheck)
                    {
                        var docFound = doctors.FirstOrDefault(m => m.ToString() == doctorToCheck);

                        if (docFound != null)
                        {
                            var seances = await GetSeances(clinic.ClinicId, date, docFound.Id);

                            if (seances != null && seances.Any())
                            {
                                foundDates.Add(new DayModel
                                {
                                    Doctor = doctorToCheck,
                                    Date = FormatDate(date),
                                    Seances = seances.Select(s => s.Time)
                                });
                            }
                        }
                    }
                }
            }

            return foundDates.GroupBy(m => m.Doctor)
                .Select(m => new TimetableModel { Service="Npc", Timetable = m.Key, Dates = m })
                .OrderBy(m => m.Timetable);
        }

        private IEnumerable<NpcClinicModel> GetClinicsAndDoctorsToCheck(IEnumerable<string> doctors)
        {
            var dict = new Dictionary<string, List<string>>();

            foreach (var doctor in doctors)
            {
                var clinics = dataContext.GetClinicsByDoctor(doctor);

                foreach (var clinic in clinics)
                {
                    if (dict.ContainsKey(clinic))
                    {
                        dict[clinic].Add(doctor);
                    }
                    else
                    {
                        dict[clinic] = new List<string> { doctor };
                    }
                }
            }

            return dict.Select(d => new NpcClinicModel { ClinicId = d.Key, Doctors = d.Value });
        }

        private async Task<IEnumerable<string>> GetDates(string clinicId)
        {
            var dates = await SendGetRequest<IEnumerable<string>>("appointableDates?affilateId=" + clinicId);
            return dates;
        }

        private async Task<IEnumerable<AppointableDoctor>> GetDoctors(string clinicId, string date)
        {
            date = date.Replace('-', '/');
            var doctors = await SendGetRequest<IEnumerable<AppointableDoctor>>($"appointableDoctors?affilateId={clinicId}&date={date}");
            return doctors;
        }

        private async Task<IEnumerable<AppointableSeance>> GetSeances(string clinicId, string date, long doctorId)
        {
            date = date.Replace('-', '/');
            var seances = await SendGetRequest<IEnumerable<AppointableSeance>>($"appointableSeances?affilateId={clinicId}&date={date}&doctorId={doctorId}");
            return seances;
        }
    }
}
