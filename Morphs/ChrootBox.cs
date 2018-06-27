using McMorph.Files;

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
    }
}