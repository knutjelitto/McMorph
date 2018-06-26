using System;
using System.Linq;

using McMorph.Files;
using McMorph.Results;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static Bash Checker()
        {
            var bash = new Bash()
                .Command($"env")
                .WithEnviroment(env =>
                {
                    env["PATH"] = "/bin:/usr/bin:/root/LiFo/bin";
                    env.Remove("PROMPT_COMMAND");
                    env.Remove("PS1");
                })
                .Run();

            bash.Run();

            foreach (var output in bash.Outputs)
            {
                Terminal.WriteLine(output);
            }

            return bash;
        }

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


        public static Bash MountOverlay(Pogo.ChrootBox box)
        {
            box.Bed.AsDirectory.Create();
            box.Changes.AsDirectory.Create();
            box.Work.AsDirectory.Create();
            box.Merged.AsDirectory.Create();
            
            var lowerdir=$"/:{box.Bed}";
            var upperdir=box.Changes;
            var workdir=box.Work;
            
            var options=$"lowerdir={lowerdir},upperdir={upperdir},workdir={workdir}";
            var command=$"mount --verbose --types overlay overlay --options {options} {box.Merged}";
            var mount = new Bash()
                .Command(command)
                .Loud()
                ;

            return mount.Run();
        }

        public static void UmountAll(Pogo.ChrootBox box)
        {
            // findmnt --list --output TARGET --noheadings

            var mounts = new Bash()
                .Command($"findmnt --list --output TARGET --noheadings | grep {box.Root}")
                .Run();

            if (!mounts.Ok)
            {
                throw Error.NewProcessError(mounts.command, mounts.ErrLines.Take(1).ToList(), mounts.ExitCode);
            }
        }
    }
}