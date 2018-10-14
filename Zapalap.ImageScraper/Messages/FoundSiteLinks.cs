using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class FoundSiteLinks
    {
        public List<string> Links { get; }

        public FoundSiteLinks(List<string> links)
        {
            Links = links;
        }
    }
}
