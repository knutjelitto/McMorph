using System;
using System.IO;

namespace McMorph.Files
{
    public static class PathNameErrors
    {
        public static FileNotFoundException FileNotFoundException(PathName path)
        {
            return new FileNotFoundException($"expected file '{path}', but it doesn't exists");
        }

        public static DirectoryNotFoundException DirectoryNotFoundException(PathName path)
        {
            return new DirectoryNotFoundException($"expected directory '{path}', but it doesn't exists");
        }

        public static FileNotFoundException EntityExistsButIsNotFile(PathName path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a file as expected");
        }

        public static FileNotFoundException EntityExistsButIsNotDirectory(PathName path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a directory as expected");
        }

        public static FileNotFoundException EntityDoesntExists(PathName path)
        {
            return new FileNotFoundException($"'{path}' doesn't exists as expected");
        }
    }
}