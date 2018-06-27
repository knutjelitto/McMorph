using McMorph.Files;

namespace McMorph.Processes
{
    public class MountPoint
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
