using System;
using System.Net;
using System.Threading.Tasks;

using McMorph.Tools;
using McMorph.Tools.Actors;
using McMorph.Files;

namespace McMorph.Downloads
{
    public class Downloader
    {
        public byte[] GetBytes(Uri uri)
        {
            var client = new WebClient();

            var basename = ((PathName)uri.LocalPath).Name;

            using (var dp = new DownloadProgress(basename))
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    dp.Advance(e.BytesReceived, e.TotalBytesToReceive);
                };

                Terminal.Write("download ", basename);

                var bytes = client.DownloadDataTaskAsync(uri).Result;

                dp.Dispose();

                Terminal.ClearLine();
                Terminal.WriteLine("download OK: ", basename);

                return bytes;
            }
        }
    }
}