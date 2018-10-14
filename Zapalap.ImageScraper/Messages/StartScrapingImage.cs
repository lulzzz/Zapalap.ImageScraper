using System;
using System.Collections.Generic;
using System.Text;

namespace Zapalap.ImageScraper.Messages
{
    public class StartScrapingImage
    {
        public string Url { get; }
        public string TargerFolderPath { get; } 

        public StartScrapingImage(string url, string targetFolderPath)
        {
            Url = url;
            TargerFolderPath = targetFolderPath;
        }
    }
}
