using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class StartScrapingSite
    {
        public string SiteUrl { get; }

        public StartScrapingSite(string siteUrl)
        {
            SiteUrl = siteUrl;
        }
    }
}
