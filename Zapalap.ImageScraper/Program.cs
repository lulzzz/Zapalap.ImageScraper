using Akka.Actor;
using System;
using Zapalap.ImageScraper.Actors;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Please provide url to start scraping");
                return;
            }

            using (var scrapyActorSystem = ActorSystem.Create("scrapy-actor-system"))
            {
                var crawlSupervisor = scrapyActorSystem.ActorOf(Props.Create(() => new CrawlSupervisor()));

                crawlSupervisor.Tell(new StartScrapingSite(args[0]));
                Console.ReadKey();
            }
        }
    }
}
