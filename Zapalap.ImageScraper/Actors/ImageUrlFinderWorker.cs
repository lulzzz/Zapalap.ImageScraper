using Akka.Actor;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class ImageUrlFinderWorker : ReceiveActor
    {
        public ImageUrlFinderWorker()
        {
            ReceiveAsync<FindImageUrls>(HandleFindImageUrls);
        }

        public async Task<bool> HandleFindImageUrls(FindImageUrls message)
        {
                var document = new HtmlDocument();
                document.LoadHtml(message.Html);

                var anchorNodes = document.DocumentNode.SelectNodes("//img[@src]");

                if (anchorNodes != null)
                {
                    var links = new List<string>();
                    foreach (var link in anchorNodes)
                    {
                        var hrefValue = link.GetAttributeValue("src", string.Empty);
                        links.Add(hrefValue);
                    }

                    Sender.Tell(new FoundImageUrls(links));
                }

            return true;
        }
    }
}
