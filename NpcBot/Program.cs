// See https://aka.ms/new-console-template for more information
// See https://habr.com/ru/post/657583/ for information about Telegram Bot
using NpcBot;
using NpcBot.Bots;
using NpcBot.Data;

Console.WriteLine("Hello, World!");

#if DEBUG
Console.WriteLine("DEBUG");
#else
Console.WriteLine("RELEASE");
#endif

Startup.Init();
var dataContext = await DataContext.Load();
var MyBot = new MessageBot(dataContext.GetBotByUserName(AppSettings.ActiveBot), AppSettings.SLEEP);
MyBot.Start();
var now = DateTime.UtcNow;

var service = new Service(dataContext, MyBot, now);
await service.Run();

var frogService = new FrogService(MyBot, now);
await frogService.Run();

#if DEBUG
Console.ReadLine();
#endif