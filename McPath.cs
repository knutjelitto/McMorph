using System;
using System.IO;

namespace McMorph
{
    public class McPath
    {
        internal readonly string Path;

        public McPath(string path)
        {
            this.Path = path;
        }

        public static McPath EnsureDirectory()
        {
            get
            {
                new McPath.EnsureDirectory(Path.GetDirectoryName(this.path));
                McPath.
            }
        }
    }
}