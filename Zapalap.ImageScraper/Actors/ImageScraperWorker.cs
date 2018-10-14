using Akka.Actor;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Zapalap.ImageScraper.Messages;

namespace Zapalap.ImageScraper.Actors
{
    public class ImageScraperWorker : ReceiveActor
    {
        private readonly string ImageUrl;
        private readonly HttpClient Http;

        public ImageScraperWorker(string imageUrl, string baseAddress)
        {
            ImageUrl = imageUrl;

            Http = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            ReceiveAsync<StartScrapingImage>(HandleStartScraping);
        }

        public async Task<bool> HandleStartScraping(StartScrapingImage message)
        {

            var response = await Http.GetAsync(ImageUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsByteArrayAsync();
            var fileName = Path.GetFileName(ImageUrl);
            var file = File.Create(Path.Combine(message.TargerFolderPath, fileName), content.Length);
            await file.WriteAsync(content);
            file.Close();
            file.Dispose();

            Context.Parent.Tell(new DoneScraping());

            return true;
        }

        protected override void PostStop()
        {
            Http.Dispose();
        }
    }
}
