using NpcBot.Bots;
using NpcBot.Data;
using NpcBot.Formatters;
using NpcBot.HttpClients;
using NpcBot.HttpClients.Calendly;
using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot
{
    public class Service
    {
        readonly DataContext dataContext;
        readonly MessageBot MyBot;
        readonly Dictionary<string, BaseClient> clients;
        readonly DateTime now;
        readonly bool SendHealthcheck;

        readonly Dictionary<string, ITimetableFormatter> Formatters = new Dictionary<string, ITimetableFormatter>
        {
            { "Npc", new TableTimeFormatter() },
            { "Calendly", new CalendarFormatter() }
        };

        public Service(DataContext dataContext, 
            MessageBot bot, 
            DateTime now,
            bool sendHealthcheck = true)
        {
            this.dataContext = dataContext;
            MyBot = bot;
            
            // клиенты для загрузки данных
            clients = RegisterClients(dataContext.GetActiveSubscriptions());

            this.now = now;
            SendHealthcheck = sendHealthcheck;
        }

        public async Task Run()
        {
            if (SendHealthcheck) await SendHealthcheckWithBot(dataContext.GetActiveHealthcheckSubscriptions());

            var loadResults = await LoadFromSites(dataContext.GetActiveSubscriptions());

            if (loadResults.Item2.Any())
            {
                await SendErrorsWithBot(dataContext.GetActiveHealthcheckSubscriptions(), loadResults.Item2);
            }

            await SendUpdatesWithBot(dataContext.GetActiveSubscriptions(), loadResults.Item1);
        }

        // TODO сюда очень просится фабрика. Но не сегодня.
        private Dictionary<string, BaseClient> RegisterClients(IEnumerable<SubscriptionModel> activeSubscriptions)
        {
            var clients = new Dictionary<string, BaseClient>();

            var serviceNames = activeSubscriptions.Select(m => m.Service).Distinct();

            if (serviceNames.Contains("Npc"))
            {
                clients.Add("Npc", new NpcClient(dataContext));
            }

            if (serviceNames.Contains("Calendly"))
            {
                clients.Add("Calendly", new CalendlyClient());
            }

            return clients;
        }

        private async Task<(List<TimetableModel>, Dictionary<string, List<string>>)> LoadFromSites(IEnumerable<SubscriptionModel> activeSubscriptions)
        {
            var all = new List<TimetableModel>();
            var errors = new Dictionary<string, List<string>>();

            foreach (var client in clients)
            {
                var allCheckingTimetables = activeSubscriptions
                    .Where(m => m.Service == client.Key)
                    .SelectMany(m => m.Timetables)
                    .Distinct();

                // Забираем данные с сайта один раз
                var allUpdates = await client.Value.GetUpdates(allCheckingTimetables);
                all.AddRange(allUpdates);

                var clientErrors = client.Value.GetErrors().ToList();
                if (clientErrors.Any())
                {
                    errors.Add(client.Key, clientErrors);
                }
            }

            return (all, errors);
        }

        private async Task SendUpdatesWithBot(IEnumerable<SubscriptionModel> activeSubscriptions, List<TimetableModel> allUpdates)
        {
            foreach (var subscription in activeSubscriptions)
            {
                var updates = allUpdates.Where(m => subscription.Timetables.Contains(m.Timetable));
                if (updates.Any())
                {
                    var sb = new StringBuilder();
                    foreach (var update in updates)
                    {
                        var formatted = Formatters[update.Service].Format(update);
                        sb.AppendLine(formatted);
                    }

                    await MyBot.Send(subscription.ChatId, sb.ToString());
                }
            }
        }

        private async Task SendHealthcheckWithBot(IEnumerable<HealthcheckSubscriptionModel> activeSubscriptions)
        {
            foreach (var sub in activeSubscriptions)
            {
                if ((now.Hour == sub.TimeUtc) && now.Minute < 15)
                {
                    await MyBot.Send(sub.ChatId, "healthcheck");
                }
            }
        }

        private async Task SendErrorsWithBot(IEnumerable<HealthcheckSubscriptionModel> activeSubscriptions, Dictionary<string, List<string>> errors)
        {
            foreach (var sub in activeSubscriptions)
            {
                foreach (var clientErrors in errors)
                {
                    if (clientErrors.Value != null && clientErrors.Value.Any())
                    {
                        var errMessage = clientErrors.Key + " \n" + string.Join('\n', clientErrors);
                        await MyBot.Send(sub.ChatId, errMessage);
                    }
                }
            }
        }
    }
}
