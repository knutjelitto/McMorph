using System;

using McMorph.Files;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static Bash MountOverlay(Pogo.TheBox box)
        {
            box.Residuum.AsDirectory.Create();
            box.Changes.AsDirectory.Create();
            box.Work.AsDirectory.Create();
            box.Root.AsDirectory.Create();
            
            var lowerdir=$"/:{box.Residuum}";
            var upperdir=box.Changes;
            var workdir=box.Work;
            
            var options=$"lowerdir={lowerdir},upperdir={upperdir},workdir={workdir}";
            var command=$"mount --verbose --types overlay overlay --options {options} {box.Root}";
            var mount = new Bash()
                .Command(command)
                .Loud()
                ;

            return mount.Run();
        }
    }
}