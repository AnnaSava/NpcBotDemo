using NpcBot.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot
{
    public class FrogService
    {
        readonly MessageBot MyBot;
        readonly DateTime now;

        public FrogService(MessageBot bot, DateTime now)
        {
            MyBot = bot;
            this.now = now;
        }

        public async Task Run()
        {
#if DEBUG
            await SendFrog(410219859, DateTime.Now);
            GetWednesdays();
#endif
        }

        async Task SendFrog(long chatId, DateTime now)
        {
            if (now.DayOfWeek != DayOfWeek.Wednesday)
                return;

            var hour = GetHour(now);

            // отправка во второй половине часа, чтобы не было "шума"
            if (now.Hour == hour && now.Minute >= 30 && now.Minute <= 59)
            {
                await MyBot.Send(chatId, "🐸");
            }
        }

        int GetHour(DateTime now)
        {
            var frogMoscowHours = new int[] { 15, 10, 14, 18, 11 };
            var week = now.Day / 7;

            return PlusMinusHour(now, frogMoscowHours[week]);
        }

        int PlusMinusHour(DateTime now, int hour)
        {
            // немного безумия
            var delta = now.Month % 3;
            if (delta == 1)
            {
                hour += 1;
            }
            if (delta == 2)
            {
                hour -= 1;
            }
            return hour;
        }

        void GetWednesdays()
        {
            var year = now.Year;

            var weds = new Dictionary<DateTime, int>();

            for (int i = 8; i <= 12; i++)
            {
                var dt = new DateTime(year, i, 1);

                while (dt.Month == i && dt.Year == year)
                {
                    if (dt.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        weds.Add(dt, GetHour(dt));
                    }
                    dt = dt.AddDays(1);
                }
            }

            foreach (var wed in weds)
            {
                Console.WriteLine($"{wed.Key.ToString("yy.MM.dd")} {wed.Value}:30");
            }
        }
    }
}
