using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.HttpClients
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;
        protected List<string> HttpErrors = new List<string>();
        protected DefaultContractResolver ContractResolver;

        private readonly string[] ruDays = new string[] { "вс", "пн", "вт", "🐸", "чт", "пт", "сб" };

#if DEBUG
        const int SLEEP = 0;
#else
        const int SLEEP = 1000;
#endif

        public BaseClient(string apiUri, NamingStrategy namingStrategy) 
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUri);
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Задаем формат названий полейв пришедшем JSON-е
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = namingStrategy
            };
        }

        public abstract Task<IEnumerable<TimetableModel>> GetUpdates(IEnumerable<string> timetablesToCheck);

        public IEnumerable<string> GetErrors()
        {
            return HttpErrors;
        }

        public string FormatDate(string date)
        {
            try
            {
                // да, мне было влом писать форматтер
                var dateArr = date.Split('-');
                var dt = new DateTime(int.Parse(dateArr[0]), int.Parse(dateArr[1]), int.Parse(dateArr[2]));
                date += $" {ruDays[(int)dt.DayOfWeek]}";
            }
            catch { }

            return date;
        }

        protected async Task<TResult> SendGetRequest<TResult>(string url)
        {
            Thread.Sleep(SLEEP);
            try
            {

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();                    
                    var deserialized = JsonConvert.DeserializeObject<TResult>(stringContent, new JsonSerializerSettings { ContractResolver = ContractResolver });

                    return deserialized;
                }

                HttpErrors.Add($"Request error {(int)response.StatusCode} {response.StatusCode} Uri: GET {url}");
            }
            catch (Exception ex)
            {

            }
            return default(TResult);
        }
    }
}
