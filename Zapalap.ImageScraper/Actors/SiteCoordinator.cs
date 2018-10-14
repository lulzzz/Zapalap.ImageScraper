using Akka.Actor;
using System;
using System.IO;
using System.Linq;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class SiteCoordinator : ReceiveActor
    {
        private readonly string SiteUrl;
        private readonly string SiteUrlBase;
        private readonly string Domain;
        private readonly string SiteDirectoryPath;
        private int OngoingImageScrapingJobs = 0;

        public SiteCoordinator(string siteUrl)
        {
            SiteUrl = siteUrl;
            Domain = new Uri(SiteUrl).Host;
            SiteUrlBase = new Uri(SiteUrl).GetLeftPart(UriPartial.Authority);
            SiteDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"/{Domain}");

            var exists = Directory.Exists(SiteDirectoryPath);

            if (!exists)
                Directory.CreateDirectory(SiteDirectoryPath);

            Receive<StartScrapingSite>(message =>
            {
                var downloader = Context.ActorOf(Props.Create(() => new SiteDownloaderWorker()));
                downloader.Tell(new DownloadSite(SiteUrl));
            });

            Receive<SiteReadyForProcessing>(message =>
            {
                var imageUrlFinder = Context.ActorOf(Props.Create(() => new ImageUrlFinderWorker()));
                imageUrlFinder.Tell(new FindImageUrls(message.SiteHtml));

                var siteLinkFinder = Context.ActorOf(Props.Create(() => new SiteLinkFinder()));
                siteLinkFinder.Tell(new FindSiteLinks(message.SiteHtml));
            });

            Receive<FoundSiteLinks>(message =>
            {
                var linksInThisDomain = message
                    .Links
                    .Where(l => l.StartsWith("/")
                        || l.StartsWith(SiteUrlBase))
                    .Select(l => l.StartsWith("/") ? $"{SiteUrlBase}{l}" : l);

                foreach (var linkInThisDomain in linksInThisDomain)
                {
                    Context.Parent.Tell(new NewSiteDiscovered(linkInThisDomain));
                }
            });

            Receive<FoundImageUrls>(message =>
            {
                foreach (var imageUrl in message.ImageUrls)
                {
                    var imageScraperWorker = Context.ActorOf(Props.Create(() => new ImageScraperWorker(imageUrl, SiteUrlBase)));
                    imageScraperWorker.Tell(new StartScrapingImage(imageUrl, $"/{Domain}"));
                    OngoingImageScrapingJobs++;
                }

                Console.WriteLine($"[{nameof(SiteCoordinator)}] Added some image scrapers. Currently: {OngoingImageScrapingJobs}");
            });

            Receive<DoneScraping>(message =>
            {
                Context.Sender.Tell(PoisonPill.Instance);
                OngoingImageScrapingJobs--;

                if (OngoingImageScrapingJobs == 0)
                {
                    Context.Parent.Tell(new DoneScraping());
                }
            });
        }
    }
}
