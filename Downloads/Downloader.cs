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
                //Console.Write("\x1B[G\x1B[K");
                Host.LineClear();
                if (e.Error != null)
                {
                    //Console.WriteLine("couldn't download {0} ({1})", basename, e.Error.Message);
                    Host.WriteLine($"couldn't download {basename} ({e.Error.Message})");
                }
                else
                {
                    //Console.WriteLine("downloaded {0}", basename);
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

        private static bool multi = false;

        private static void Bar(string address, long received, long total)
        {
            if (multi)
            {
                return;
            }
            multi = true;
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

            //Console.Write("\x1B[G{0}: [{1}]", head, bar);
            Host.LineHome();
            Host.Write(head, ": [", bar, "]");
            multi = false;
        }
    }
}