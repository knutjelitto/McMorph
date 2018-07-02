using System;
using System.Collections.Generic;

using McMorph.Tools;
using McMorph.Files;

using McMorph.Morphs;

namespace McMorph.Processes
{
    public class Mounter : IDisposable
    {
        private readonly ChrootBox box;
        private Progress progress;
        private Stack<MountPoint> mountPoints = new Stack<MountPoint>();

        public Mounter(ChrootBox box)
        {
            this.box = box;
            this.progress = new Progress();
        }

        public void MountOverlay()
        {
            this.box.Base.CreateDirectory();
            this.box.Changes.CreateDirectory();
            this.box.Work.CreateDirectory();
            this.box.Merged.CreateDirectory();
            
            var lowerdir=$"/:{this.box.Base}:{this.box.Pogo.Root}";
            var upperdir=box.Changes;
            var workdir=box.Work;
            
            var options=$"lowerdir={lowerdir},upperdir={upperdir},workdir={workdir}";
            var command=$"mount --verbose --types overlay overlay --options {options} {box.Merged}";
            var mount = MountRun(command);
            this.mountPoints.Push(new MountPoint(box.Merged, false));

            Bash.ThrowOnError(mount);
        }

        public void SysfsMount(UPath target)
        {
            var bind = MountRun($"mount --verbose --types sysfs sysfs {target}");

            this.mountPoints.Push(new MountPoint(target, false));

            Bash.ThrowOnError(bind);
        }

        public void ProcfsMount(UPath target)
        {
            var bind = MountRun($"mount --verbose --types proc proc {target}");

            this.mountPoints.Push(new MountPoint(target, false));

            Bash.ThrowOnError(bind);
        }

        public void BindMount(UPath source, UPath target)
        {
            var bind = MountRun($"mount --verbose --bind {source} {target}");

            this.mountPoints.Push(new MountPoint(target, false));

            Bash.ThrowOnError(bind);
        }

        public void RecursiveBindMount(UPath source, UPath target)
        {
            var bind = MountRun($"mount --verbose --rbind {source} {target}");

            this.mountPoints.Push(new MountPoint(target, true));

            Bash.ThrowOnError(bind);
        }

        public void MountDone()
        {
            this.Dispose();
        }

        public void UnMount(ChrootBox box)
        {
            using (var progress = new Progress())
            {
                while (this.mountPoints.Count > 0)
                {
                    var mountPoint = this.mountPoints.Pop();

                    try
                    {
                        UnMountOne(box, mountPoint, progress);
                    }
                    catch (Exception exception)
                    {
                        Terminal.ClearLine();
                        Terminal.WriteLine("error unmounting ", mountPoint.Path, ":");
                        Terminal.WriteLine(exception.Message);
                    }
                }
            }
        }

        private void UnMountOne(ChrootBox box, MountPoint mountPoint, Progress progress)
        {
            var flag = (mountPoint.Recursive ? "--recursive" : string.Empty);

            var umount = new Bash()
                .Command($"umount --verbose {flag} {mountPoint.Path}")
                .WithProgress(progress)
                .Run();

            Bash.ThrowOnError(umount);
        }

        private Bash MountRun(string command)
        {
            //System.Threading.Tasks.Task.Delay(30).Wait();

            return new Bash()
                .Command(command)
                .WithProgress(this.progress)
                .Run();
        }

        public void Dispose()
        {
            if (this.progress != null)
            {
                this.progress.Dispose();
                this.progress = null;
            }
        }

        private class MountPoint
        {
            public MountPoint(UPath path, bool recursive)
            {
                Path = path;
                Recursive = recursive;
            }

            public UPath Path { get; }
            public bool Recursive { get; }
        }
    }
}