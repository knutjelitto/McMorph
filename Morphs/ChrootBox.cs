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

            (Base / "root" / ".chroot").TouchFile();
            "/root/.profile".CopyTo(Base / "root/.profile");
            "/root/.bashrc".CopyTo(Base / "root/.bashrc");

            // links
            (Base / "bin/sh").SymbolicLinkTo("bash", true);
            (Base / "root/Pogo/Data").SymbolicLinkTo("/Data", true);

            // files with content
            //(Base / "root" / ".chroot-exec").WriteAllText("exit 12");
        }
    }
}