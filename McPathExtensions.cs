using System;
using System.IO;

namespace McMorph
{
    public static class McPathExtensions
    {
        public static McPath EnsureAsDirectory(this McPath mcPath)
        {
            if (!Directory.Exists(mcPath.Path))
            {

            }
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