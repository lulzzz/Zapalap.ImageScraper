using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class NewSiteDiscovered
    {
        public string Link { get;  }

        public NewSiteDiscovered(string link)
        {
            Link = link;
        }
    }
}
