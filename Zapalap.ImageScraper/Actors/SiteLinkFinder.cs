using Akka.Actor;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class SiteLinkFinder : ReceiveActor
    {
        public SiteLinkFinder()
        {
            Receive<FindSiteLinks>(message =>
            {
                var document = new HtmlDocument();
                document.LoadHtml(message.Html);

                var anchorNodes = document.DocumentNode.SelectNodes("//a[@href]");

                if (anchorNodes != null)
                {
                    var links = new List<string>();
                    foreach (var link in anchorNodes)
                    {
                        var hrefValue = link.GetAttributeValue("href", string.Empty);
                        links.Add(hrefValue);
                    }

                    Sender.Tell(new FoundSiteLinks(links));
                }
            });
        }
    }
}
