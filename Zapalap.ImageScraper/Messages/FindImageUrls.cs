using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class FindImageUrls
    {
        public string Html { get; }

        public FindImageUrls(string html)
        {
            Html = html;
        }
    }
}
