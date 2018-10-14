using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class CrawlSupervisor : ReceiveActor
    {
        private int SiteCoordinatorCount = 0;
        
        public CrawlSupervisor()
        {
            Receive<StartScrapingSite>(message =>
            {
                var siteCoordinator = Context.ActorOf(Props.Create(() => new SiteCoordinator(message.SiteUrl)));
                siteCoordinator.Tell(message);
            });

            Receive<NewSiteDiscovered>(message =>
            {
                Console.WriteLine($"[{nameof(CrawlSupervisor)}] New site discovered: {message.Link}");
                var siteCoordinator = Context.ActorOf(Props.Create(() => new SiteCoordinator(message.Link)), $"SiteCoordinator:{++SiteCoordinatorCount}");
                siteCoordinator.Tell(new StartScrapingSite(message.Link));
            });

            Receive<DoneScraping>(message =>
            {
                Console.WriteLine($"[{nameof(CrawlSupervisor)}] Site coordinator done scraping {Context.Sender.Path}");
                Context.Sender.Tell(PoisonPill.Instance);
            });
        }
    }
}
