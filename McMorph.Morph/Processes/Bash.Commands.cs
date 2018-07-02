using System;
using System.Collections.Generic;
using System.Linq;

using McMorph.Files;
using McMorph.Morphs;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static Bash RemoveDirectory(UPath directory)
        {
            var bash = new Bash()
                .Command($"rm --recursive --force --verbose \"{directory}\"")
                .WithProgress()
                ;

            return bash.Run();
        }

        public static Bash TarExtract(UPath archivePath, UPath extractPath)
        {
            var bash = new Bash()
                .Command($"tar --extract --verbose --owner=0 --group=0 --file \"{archivePath}\"")
                .Directory(extractPath)
                .WithProgress()
                ;

            return bash.Run();
        }

        public static void DoTheChrootBash(Pogo pogo)
        {
            Mono.Unix.Native.Syscall.chroot(pogo.Box.Merged.FullName);

            var bash = new Bash()
                .Command($"bash")
                .Directory("/root")
                .Interactive()
                .Run();
        }
    }
}