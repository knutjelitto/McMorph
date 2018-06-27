using System;
using System.Collections.Generic;
using System.Linq;

using McMorph.Files;
using McMorph.Morphs;
using McMorph.Results;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static void MountOverlay(ChrootBox box, Stack<MountPoint> mountPoints)
        {
            box.Base.AsDirectory.Create();
            box.Changes.AsDirectory.Create();
            box.Work.AsDirectory.Create();
            box.Merged.AsDirectory.Create();
            
            var lowerdir=$"/:{box.Base}";
            var upperdir=box.Changes;
            var workdir=box.Work;
            
            var options=$"lowerdir={lowerdir},upperdir={upperdir},workdir={workdir}";
            var command=$"mount --verbose --types overlay overlay --options {options} {box.Merged}";
            var mount = new Bash()
                .Command(command)
                .Loud()
                .Run()
                ;

            mountPoints.Push(new MountPoint(box.Merged, false));

            ThrowOnError(mount);
        }

        public static void SysfsMount(UPath target, Stack<MountPoint> mountPoints)
        {
            var bind = new Bash()
                .Command($"mount --verbose --types sysfs sysfs {target}")
                .Loud()
                .Run();

            mountPoints.Push(new MountPoint(target, false));

            ThrowOnError(bind);
        }

        public static void ProcfsMount(UPath target, Stack<MountPoint> mountPoints)
        {
            var bind = new Bash()
                .Command($"mount --verbose --types proc proc {target}")
                .Loud()
                .Run();

            mountPoints.Push(new MountPoint(target, false));

            ThrowOnError(bind);
        }

        public static void BindMount(UPath source, UPath target, Stack<MountPoint> mountPoints)
        {
            var bind = new Bash()
                .Command($"mount --verbose --bind {source} {target}")
                .Loud()
                .Run();

            mountPoints.Push(new MountPoint(target, false));

            ThrowOnError(bind);
        }

        public static void RecursiveBindMount(UPath source, UPath target, Stack<MountPoint> mountPoints)
        {
            var bind = new Bash()
                .Command($"mount --verbose --rbind {source} {target}")
                .Loud()
                .Run();

            mountPoints.Push(new MountPoint(target, true));

            ThrowOnError(bind);
        }

        public static void UnMountAll(ChrootBox box, Stack<MountPoint> mountPoints)
        {
            //var mounts = new Bash()
            //    .Command($"findmnt --list --output TARGET --noheadings | grep {box.Root}")
            //    .Run();
            //ThrowOnError(mounts);

            while (mountPoints.Count > 0)
            {
                var mountPoint = mountPoints.Pop();

                try
                {
                    UnMount(box, mountPoint);
                }
                catch (Exception exception)
                {
                    Terminal.ClearLine();
                    Terminal.WriteLine("error unmounting ", mountPoint.Path, ":");
                    Terminal.WriteLine(exception.Message);
                }
            }
        }

        public static void UnMount(ChrootBox box, MountPoint mountPoint)
        {
            var command = mountPoint.Recursive
                ? $"umount --verbose --recursive {mountPoint.Path}"
                : $"umount --verbose {mountPoint.Path}";

            var umount = new Bash()
                .Command(command)
                .Loud()
                .Run();

            ThrowOnError(umount);
        }
    }
}