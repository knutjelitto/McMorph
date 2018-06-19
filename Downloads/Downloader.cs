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
                Console.WriteLine("\x1B[G\x1B[K");
            };
            client.DownloadProgressChanged += (s, e) =>
            {
                Bar(basename, e.BytesReceived, e.TotalBytesToReceive);
            };
            return client.DownloadDataTaskAsync(address);
        }

        private static bool multi = false;

        private static void Bar(string address, long received, long total)
        {
            if (multi)
            {
                return;
            }
            multi = true;
            var prefix = 1 * Console.WindowWidth / 6 - 2;
            var barLength = Console.WindowWidth - prefix - 22;
            int chars;
            if (total > 0)
            {
                chars = (int)(barLength * received / total);
            }
            else
            {
                chars = 0;
            }

            var bar = new string('=', chars) + ">" + new string('.', barLength - chars);
            //var bar = $"{chars}>{barLength - chars}";

            string head;

            if (address.Length >= prefix)
            {
                head = "..." + address.Substring(address.Length - prefix - 3);
            }
            else
            {
                head = new string(' ', prefix - address.Length) + address;
            }

            Console.Write("\x1B[G{0}: [{1}]", head, bar);
            multi = false;
        }
    }
}