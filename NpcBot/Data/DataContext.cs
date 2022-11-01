using Newtonsoft.Json;
using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Data
{
    public class DataContext
    {        
        const string DataStorageFileName = "datastorage.json";

        public ICollection<BotModel> Bots { get; set; }

        public ICollection<NpcClinicModel> NpcClinics { get; set; }

        public ICollection<SubscriptionModel> Subscriptions { get; set; }

        public ICollection<HealthcheckSubscriptionModel> HealthcheckSubscriptions { get; set; }

        public static async Task<DataContext> Load()
        {
            string path = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, DataStorageFileName));

            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                var data = await sr.ReadToEndAsync();
                var deserialized = JsonConvert.DeserializeObject<DataContext>(data);
                return deserialized;
            }
        }

        public BotModel GetBotByUserName(string botUserName)
        {
            return Bots.FirstOrDefault(m => m.UserName == botUserName);
        }

        public IEnumerable<SubscriptionModel> GetActiveSubscriptions() => Subscriptions.Where(m => m.IsActive);

        public IEnumerable<HealthcheckSubscriptionModel> GetActiveHealthcheckSubscriptions() => HealthcheckSubscriptions.Where(m => m.IsActive);

        public IEnumerable<string> GetClinicsByDoctor(string doctorName)
        {
            return NpcClinics.Where(m => m.Doctors.Contains(doctorName)).Select(m => m.ClinicId);
        }
    }
}
