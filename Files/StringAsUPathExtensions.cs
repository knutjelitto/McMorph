using System;

namespace McMorph.Files
{
    public static class StringAsUPathExtensions
    {
        public static UPath AsPath(this string path)
        {
            return new UPath(path);
        }

        public static void CreateDirectory(this string path)
        {
            FileSystem.Instance.CreateDirectory(path);
        }

        public static void CopyTo(this string source, UPath destination)
        {
            FileSystem.Instance.CopyFile(source, destination, true);
        }

        public static void Touch(this string path)
        {
            var file = path.AsPath().AsFile;

            if (!file.Exists)
            {
                file.Create().Close();
            }
            file.LastWriteTime = DateTime.Now;
        }
    }
}