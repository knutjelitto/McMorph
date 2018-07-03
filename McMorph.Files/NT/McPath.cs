using System;

namespace McMorph.Files
{
    public static class McPath
    {
        public static IPath Path(string path)
        {
            throw new NotImplementedException();
        }

        public static PurePath PurePath(string path)
        {
            throw new NotImplementedException();
        }

        public static PurePosixPath PurePosixPath(string path)
        {
            return McMorph.Files.PurePosixPath.Parse(path);
        }

        public static PosixPath PosixPath(string path)
        {
            throw new NotImplementedException();
        }


        public static PureWindowsPath PureWindowsPath(string path)
        {
            throw new NotImplementedException();
        }

        public static WindowsPath WindowsPath(string path)
        {
            throw new NotImplementedException();
        }
    }
}