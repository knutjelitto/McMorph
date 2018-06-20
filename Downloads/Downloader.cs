using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace McMorph.Downloads
{
    public class Downloader
    {
        public Task<byte[]> GetBytes(string address)
        {
            var client = new WebClient();

            var basename = Path.GetFileName(address);

            client.DownloadDataCompleted += (s, e) =>
            {
                Host.LineClear();
                if (e.Error != null)
                {
                    Host.WriteLine($"couldn't download {basename} ({e.Error.Message})");
                }
                else
                {
                    Host.WriteLine("downloaded ", basename);
                }
            };
            client.DownloadProgressChanged += (s, e) =>
            {
                Bar(basename, e.BytesReceived, e.TotalBytesToReceive);
            };
            Host.Write("download ", basename);
            return client.DownloadDataTaskAsync(address);
        }

        private static void Bar(string address, long received, long total)
        {
            var width = Console.WindowWidth - 12;
            var prefix = width / 4;
            prefix = Math.Min(address.Length, prefix);
            var inner = (width - prefix) - 5; // account for ': [' + '] '
            inner = inner - "download ".Length;
            int chars;
            if (total > 0)
            {
                chars = (int)(inner * received / total);
            }
            else
            {
                chars = 0;
            }

            var bar = new string('=', chars) + ">" + new string('.', inner - chars);

            string head;

            if (address.Length > prefix)
            {
                head = "..." + address.Substring(address.Length - prefix - 3);
            }
            else
            {
                head = address + new string(' ', prefix - address.Length);
            }
            head = "download " + head;

            Host.Write("\r", head, ": [", bar, "]");
        }
    }
}