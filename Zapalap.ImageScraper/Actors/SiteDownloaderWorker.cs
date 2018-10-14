using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class SiteDownloaderWorker : ReceiveActor
    {
        public SiteDownloaderWorker()
        {
            ReceiveAsync<DownloadSite>(HandleDownloadSite);
        }

        public async Task<bool> HandleDownloadSite(DownloadSite message)
        {
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(message.Url);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();
                Context.Sender.Tell(new SiteReadyForProcessing(html));
            }

            return true;
        }
    }
}
