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
            Pogo = new Pogo();

            foreach (var arg in args)
            {
                if (arg == "--do-the-chroot--")
                {
                    Bash.DoTheChrootBash(Pogo);
                    return 127;
                }
            }

            Terminal.Write("reading recipes ... ");
            var morphs = MorphCollection.Populate(
                Pogo, 
                IsLinux
                    ? "/root/McMorph/Repository"
                    : Environment.CurrentDirectory.AsPath() / "Repository");
            Terminal.ClearLine();
            
            morphs.Download(false);
            morphs.Extract(false);

            Pogo.Box.Mount(true);

#if false
            //Console.Write("any key ...");
            //Console.ReadKey(true);
            //Console.WriteLine();
#endif
            return 0;
        }
    }
}
