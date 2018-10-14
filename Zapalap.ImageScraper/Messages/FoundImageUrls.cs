using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class FoundImageUrls
    {
        public List<string> ImageUrls { get; }

        public FoundImageUrls(List<string> imageUrls)
        {
            ImageUrls = imageUrls;
        }
    }
}
