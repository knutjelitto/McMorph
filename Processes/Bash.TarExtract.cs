using System;

using McMorph.Files;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static Bash TarExtract(UPath archivePath, UPath extractPath)
        {
            var bash = new Bash()
                .Command($"tar --extract --verbose --owner=0 --group=0 --file \"{archivePath}\"")
                .Directory(extractPath)
                .WithProgress()
                ;

            return bash.Run();
        }
    }
}