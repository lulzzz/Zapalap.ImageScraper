using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class DownloadSite 
    {
        public string  Url { get; }

        public DownloadSite(string url)
        {
            Url = url;
        }
    }
}
