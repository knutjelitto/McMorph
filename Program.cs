using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Results;
using McMorph.Recipes;
using McMorph.Downloads;
using McMorph.Processes;
using McMorph.Morphs;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph
{
    class Program
    {
        public static Pogo Pogo;

        static void Main(string[] args)
        {

            Pogo = new Pogo();

            var morphs = MorphCollection.Populate();
            
            //if (false)
            //{
            //    Console.Write("Find ALL: ");
            //    Console.CursorVisible = false;
            //    //var find = new Bash().Command("find / -type f").Run();;
            //    var log = new CollectSink();
            //    var errors = new CollectSink();
            //    var find = new Bash()
            //        .Command("find /Pogo/Data/Archives -type f")
            //        .StdOut(new ProgressSink(log))
            //        .StdErr(new TeeSink(log, errors))
            //        .Run();
            //    Console.CursorVisible = true;
            //}

            Download(morphs.Upstreams).Wait();

            Console.Write("any key ...");
            Console.ReadKey(true);
            Console.WriteLine();
        }


        static async Task Download(IEnumerable<string> upstreams)
        {
            var downloader = new Downloader();

            foreach (var upstream in upstreams)
            {
                var filepath = Pogo.ArchivesPath(new Uri(upstream));
                
                if (!File.Exists(filepath))
                {
                    try
                    {
                        var bytes = await downloader.GetBytes(upstream);

                        Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                        File.WriteAllBytes(filepath, bytes);
                    }
                    catch {}
                }
            }
        }
    }
}
