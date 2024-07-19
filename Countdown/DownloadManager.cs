using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace Countdown
{
    public class DownloadManager
    {
        private readonly HttpClient _httpClient;

        public DownloadManager()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> DownloadAsync(string dictionary, string url)
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, dictionary);

            using (var response = await _httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
                else
                {
                    throw new Exception("Failed to download dictionary.");
                }
            }

            return filePath;
        }
    }
}
