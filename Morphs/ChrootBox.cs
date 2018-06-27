using System.Collections.Generic;

using McMorph.Files;
using McMorph.Processes;

namespace McMorph.Morphs
{
    public class ChrootBox
    {
        public ChrootBox(Pogo pogo, UPath boxRoot)
        {
            Pogo = pogo;
            Root = boxRoot;
        }

        public Pogo Pogo { get; }
        public UPath Root { get; }
        public UPath Changes => Root / "Changes";
        public UPath Work => Root / "Work";
        public UPath Base => Root / "Base";
        public UPath Merged => Root / "Merged";

        public void Prepare(bool force)
        {
            if (force)
            {
                var directory = Base.AsDirectory;
                if (directory.Exists)
                {
                    Base.AsDirectory.Delete(true);
                }
            }

            // directories
            //            
            Base.CreateDirectory();

            var directories = new string[]
            {
                "root",
                "root/Pogo",
                "root/McMorph",
                "bin",
                "sbin",
                "lib",
                "lib64",
                "etc",
                "usr",
                "usr/bin",
                "usr/sbin",
                "usr/lib",
                "usr/include",
                "usr/libexec",
                "usr/share",
                "usr/share/doc",
                "usr/share/info",
                "usr/share/man",
                "tmp",
                "var",
                "dev",
                "proc",
                "sys",
                "var",
                "run",
                "run/lock",
                "var/cache",
                "var/lib",
                "var/log",
                "var/tmp",
            };
            foreach (var dir in directories)
            {
                (Base / dir).CreateDirectory();
            }

            // files
            //
            (Base / "etc/hostname").TouchFile();
            (Base / "etc/hosts").TouchFile();
            (Base / "etc/resolv.conf").TouchFile();

            // links
            (Base / "bin/sh").SymbolicLinkTo("bash", true);
        }

        public void Mount()
        {
            var mountPoints = new Stack<MountPoint>();

            MountOverlay(mountPoints);
            MountCommons(mountPoints);
            Self.Exec();    

            Bash.UnMountAll(this, mountPoints);
        }

        private void MountOverlay(Stack<MountPoint> mountPoints)
        {
            Bash.MountOverlay(this, mountPoints);
        }

        private void MountCommons(Stack<MountPoint> mountPoints)
        {
            Bash.BindMount("/root/Pogo", Merged / "root/Pogo", mountPoints);
            Bash.BindMount("/root/McMorph", Merged / "root/McMorph", mountPoints);
            Bash.RecursiveBindMount("/dev", Merged / "dev", mountPoints);
            Bash.SysfsMount(Merged / "sys", mountPoints);
            //Bash.RecursiveBindMount("/proc", Merged / "proc", mountPoints);
            Bash.ProcfsMount(Merged / "proc", mountPoints);
            Bash.BindMount("/etc/hostname", Merged / "etc/hostname", mountPoints);
            Bash.BindMount("/etc/hosts", Merged / "etc/hosts", mountPoints);
            Bash.BindMount("/etc/resolv.conf", Merged / "etc/resolv.conf", mountPoints);
        }
    }
}