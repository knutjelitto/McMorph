using System;
using System.IO;

namespace McMorph.Files
{
    public static class FilesError
    {
        public static FileNotFoundException NewFileNotFoundException(UPath path)
        {
            return new FileNotFoundException($"expected file '{path}', but it doesn't exists");
        }

        public static DirectoryNotFoundException NewDirectoryNotFoundException(UPath path)
        {
            return new DirectoryNotFoundException($"expected directory '{path}', but it doesn't exists");
        }

        public static FileNotFoundException NewExistsButIsNotFile(UPath path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a file as expected");
        }

        public static FileNotFoundException NewExistsButIsNotDirectory(UPath path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a directory as expected");
        }

        public static FileNotFoundException NewEntryDoesntExists(UPath path)
        {
            return new FileNotFoundException($"'{path}' doesn't exists as expected");
        }
    }
}