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

        public McPath EnsureDirectory
        {
            get
            {
                return this;
            }
        }
    }
}
