using System;

namespace McMorph.Files
{
    public static class UPathEntryExtensions
    {
        public static void CreateDirectory(this UPath path)
        {
            FileSystem.Instance.CreateDirectory(path);
        }

        public static void TouchFile(this UPath path)
        {
            if (!FileSystem.Instance.FileExists(path))
            {
                path.CreateFile().Close();

            }
            FileSystem.Instance.SetLastWriteTime(path, DateTime.Now);
        }

        public static void SymbolicLinkTo(this UPath link, UPath value, bool force = false)
        {
            FileSystem.Instance.CreateSymbolicLink(link, value, force);
        }

        public static void SetText(this UPath link, string text)
        {
            link.WriteAllText(text);
        }
    }
}