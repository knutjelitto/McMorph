using System;

namespace McMorph.Files
{
    public static class StringAsUPathExtensions
    {
        public static UPath AsPath(this string path)
        {
            return new UPath(path);
        }

        public static void CopyTo(this string source, UPath destination)
        {
            FileSystem.Instance.CopyFile(source, destination, true);
        }
    }
}