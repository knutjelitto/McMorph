using System;
using System.Net;
using System.Threading.Tasks;

using McMorph.Actors;
using McMorph.Files;

namespace McMorph.Downloads
{
    public class Downloader
    {
        public byte[] GetBytes(Uri uri)
        {
            // wget https://ftpmirror.gnu.org/gnu/hello/hello-2.10.tar.gz

            var client = new WebClient();

            var basename = ((UPath)uri.LocalPath).GetName();

            var progress = new ActionActor();

            client.DownloadProgressChanged += (s, e) =>
            {
                progress.Send(() => Definite(basename, e.BytesReceived, e.TotalBytesToReceive));
                //progress.Send(() => Indefinite(basename, e.BytesReceived));
            };

            Terminal.Write("download ", basename);

            try
            {
                var bytes = client.DownloadDataTaskAsync(uri).Result;

                progress.Done();
                Terminal.ClearLine();
                Terminal.WriteLine("downloaded ", basename);

                return bytes;
            }
            catch (Exception exception)
            {
                progress.Done();
                Terminal.ClearLine();

                Terminal.WriteLine($"couldn't download {basename} ({exception.Message})");

                return null;
            }
        }

        private static string DBar(long received, long total, int width)
        {
            int chars = (int)(width * received / total);

            var bar = new string('=', chars) + ">" + new string('.', width - chars);

            return bar;
        }

        private static string IBar(long total, int width)
        {
            const string boat = "<==>";

            int offset = ((int)(total / (256 * 1024))) % (width - boat.Length);

            var bar = new string('.', offset) + boat + new string('.', width - offset - boat.Length);

            return bar;
        }

        private static string Start(string prefix)
        {
            return "download " + prefix + ": [";
        }

        private static string End()
        {
            return "]";
        }

        private static int Inner(string start, string end)
        {
            return Math.Min(30, Terminal.Width - start.Length - end.Length - 10);
        }

        private static void Indefinite(string prefix, long received)
        {
            var start = Start(prefix);
            var end = End();
            
            var width = Inner(start, end);

            if (width >= 10)
            {
                var bar = IBar(received, width);

                Terminal.GotoLineHome();
                Terminal.Write(start, bar, end);
            }
        }

        private static void Definite(string prefix, long received, long total)
        {
            var start = Start(prefix);
            var end = End();
            
            var width = Inner(start, end);

            if (width >= 10)
            {
                var bar = DBar(received, total, width);

                Terminal.GotoLineHome();
                Terminal.Write(start, bar, end);
            }
        }
    }
}