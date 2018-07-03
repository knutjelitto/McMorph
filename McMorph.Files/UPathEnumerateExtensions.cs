using System.Collections.Generic;
using System.IO;

using McMorph.Tools;

namespace McMorph.Files
{
    public static class UPathEnumerateExtensions
    {
        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateDirectories(this UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.Directory))
            {
                yield return subPath;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of file names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateFiles(this UPath path)
        {
            return path.EnumerateFiles();
        }

        /// <summary>
        /// Returns an enumerable collection of file names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateFiles(this UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.File))
            {
                yield return subPath;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of <see cref="FileEntry"/> from the specified path.</returns>
        public static IEnumerable<FileEntry> EnumerateFileEntries(this UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in EnumerateFiles(path, searchPattern, searchOption))
            {
                yield return new FileEntry(subPath);
            }
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="DirectoryEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of <see cref="DirectoryEntry"/> from the specified path.</returns>
        public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(this UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in path.EnumerateDirectories(searchPattern, searchOption))
            {
                yield return new DirectoryEntry(subPath);
            }
        }
    }
}