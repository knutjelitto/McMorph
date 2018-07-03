using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Tools;
using McMorph.Files;

using McMorph.Recipes;
using McMorph.Downloads;
using McMorph.Processes;
using McMorph.Morphs;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph
{
    public static class Program
    {
        public const string ChrootIntro = "@do@the@chroot@";
        public static Pogo Pogo;

        public static bool IsLinux => System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

        static int Main(string[] args)
        {
            // https://github.com/landley/mkroot
            // https://wiki.musl-libc.org/projects-using-musl.html
            // http://www.dragora.org/repo.fsl/doc/trunk/www/index.md
            // toolchain build order: linux-api-headers->glibc->binutils->gcc->binutils->glibc

            Pogo = new Pogo();

            foreach (var arg in args)
            {
                if (arg == ChrootIntro)
                {
                    Bash.DoTheChrootBash(Pogo);
                    return 127;
                }
            }

            Terminal.Write("reading recipes ... ");
            var morphs = Morphs.Morphs.Populate(
                Pogo,
                IsLinux
                    ? "/root/McMorph/McMorph.Morph/Repository"
                    : ((PathName)Environment.CurrentDirectory) / "Repository");
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

        static int ChrootMain(List<string> arguments)        
        {
            return 0;
        }
    }
}
