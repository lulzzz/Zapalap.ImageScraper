using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class SiteReadyForProcessing
    {
        public string SiteHtml { get; }

        public SiteReadyForProcessing(string siteHtml)
        {
            SiteHtml = siteHtml;
        }
    }
}
