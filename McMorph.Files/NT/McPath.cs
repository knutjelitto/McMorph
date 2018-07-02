using System;

namespace McMorph.Files
{
    public static class McPath
    {
        public static IPath Path(string path)
        {
            return null;
        }

        public static PurePath PurePath(string path)
        {
            return null;
        }

        public static PurePosixPath PurePosixPath(string path)
        {
            return McMorph.Files.PurePosixPath.Parse(path);
        }

        public static PureWindowsPath PureWindowsPath(string path)
        {
            return null;
        }
    }
}