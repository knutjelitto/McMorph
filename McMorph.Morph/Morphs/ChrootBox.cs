using System.Collections.Generic;

using McMorph.Tools;
using McMorph.Files;
using McMorph.Processes;

namespace McMorph.Morphs
{
    public class ChrootBox
    {
        public ChrootBox(Pogo pogo, PathName boxRoot)
        {
            Pogo = pogo;
            Root = boxRoot;
        }

        public Pogo Pogo { get; }
        public PathName Root { get; }
        public PathName Changes => Root / "Changes";
        public PathName Work => Root / "Work";
        public PathName Base => Root / "Base";
        public PathName Merged => Root / "Merged";

        public void Mount(bool force)
        {
            Terminal.Write("preparing ... ");
            Prepare(force);
            Terminal.ClearLine();

            var mounter = new Mounter(this);

            Terminal.Write("mounting ... ");
            mounter.MountOverlay();
            mounter.BindMount("/root/McMorph", Merged / "root/McMorph");
            mounter.RecursiveBindMount("/dev", Merged / "dev");
            mounter.SysfsMount(Merged / "sys");
            mounter.ProcfsMount(Merged / "proc");
            mounter.BindMount("/etc/hostname", Merged / "etc/hostname");
            mounter.BindMount("/etc/hosts", Merged / "etc/hosts");
            mounter.BindMount("/etc/resolv.conf", Merged / "etc/resolv.conf");
            mounter.MountDone();

            Self.Exec();

            Terminal.Write("un-mounting ... ");
            mounter.UnMount(this);
        }

        private void Prepare(bool force)
        {
            if (force)
            {
                if (Base.ExistsDirectory())
                {
                    Base.DeleteDirectory(true);
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

            (Base / "root" / ".chroot").TouchFile();
            (Base / "root/.profile").CopyFileFrom("/root/.profile");
            (Base / "root/.bashrc").CopyFileFrom("/root/.bashrc");

            // links
            (Base / "bin/sh").SymlinkTo("bash", true);
            (Base / "root/Pogo/Data").SymlinkTo("/Data", true);

            // files with content
            //(Base / "root" / ".chroot-exec").WriteAllText("exit 12");
        }
    }
}