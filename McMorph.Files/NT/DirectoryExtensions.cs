using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace McMorph.Files
{
    public static class DirectoryExtensions
    {
        public static bool ExistsDirectory(this PathName path)
        {
            return Directory.Exists(path.Full);
        }

        public static void DeleteDirectory(this PathName path, bool recursive = false)
        {
            Directory.Delete(path.Full, recursive);
        }

        public static void CreateDirectory(this PathName path)
        {
            Directory.CreateDirectory(path.Full);
        }

        public static void MoveDirectory(this PathName source, PathName destination)
        {
            Directory.Move(source.Full, destination.Full);
        }

        public static IEnumerable<PathName> EnumerateFiles(this PathName path)
        {
            return Directory.EnumerateFiles(path.Full).Select(filePath => PathName.Path(filePath));
        }

        public static IEnumerable<PathName> EnumerateDirectories(this PathName path)
        {
            return Directory.EnumerateDirectories(path.Full).Select(filePath => PathName.Path(filePath));
        }
    }
}