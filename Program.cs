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
using McMorph.Files;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph
{
    class Program
    {
        public static Pogo Pogo;

        public static bool IsLinux => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

        static int Main(string[] args)
        {
            foreach (var arg in args)
            {
                Terminal.WriteLine("argument: ", arg);
                if (arg == "chroot")
                {
                    Self.Exec();
                    return 1;
                }
                if (arg == "touch")
                {
                    "/root/XXXX".Touch();
                    return 1;
                }
            }

            Pogo = new Pogo();

            var morphs = MorphCollection.Populate(
                Pogo, 
                IsLinux
                    ? "/root/McMorph/Repository"
                    : Environment.CurrentDirectory.AsPath() / "Repository");
            Terminal.ClearLine();
            Terminal.WriteLine("reading OK");
            
            morphs.Download(false);
            morphs.Extract(false);

            Pogo.Box.Prepare(true);
#if false

            //Bash.MountOverlay(Pogo.Box);
            //Bash.UnMountAll(Pogo.Box);

            //Console.Write("any key ...");
            //Console.ReadKey(true);
            //Console.WriteLine();
#endif

            return 0;
        }
    }
}
