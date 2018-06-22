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

            var morphs = MorphCollection.Populate(Pogo);
            
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

            morphs.Extract().Wait();
            morphs.Download();

            Console.Write("any key ...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
