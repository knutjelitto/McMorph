using System;
using System.Diagnostics;

using McMorph.Files;

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
    }
}