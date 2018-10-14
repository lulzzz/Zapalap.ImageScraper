using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class FindSiteLinks
    {
        public string Html { get; }

        public FindSiteLinks(string html)
        {
            Html = html;
        }
    }
}
