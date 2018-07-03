using System;
using System.IO;

namespace McMorph.Files
{
    public static class FilesError
    {
        public static FileNotFoundException FileNotFoundException(PathName path)
        {
            return new FileNotFoundException($"expected file '{path}', but it doesn't exists");
        }

        public static DirectoryNotFoundException NewDirectoryNotFoundException(PathName path)
        {
            return new DirectoryNotFoundException($"expected directory '{path}', but it doesn't exists");
        }

        public static FileNotFoundException NewExistsButIsNotFile(PathName path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a file as expected");
        }

        public static FileNotFoundException NewExistsButIsNotDirectory(PathName path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a directory as expected");
        }

        public static FileNotFoundException NewEntryDoesntExists(PathName path)
        {
            return new FileNotFoundException($"'{path}' doesn't exists as expected");
        }
    }
}