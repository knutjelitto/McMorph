// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using McMorph.Results;

namespace McMorph.Files
{
    /// <summary>
    ///     Extension methods for <see cref="FileSystem" />
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        ///     Copies a file between two filesystems.
        /// </summary>
        /// <param name="fs">The source filesystem</param>
        /// <param name="destFileSystem">The destination filesystem</param>
        /// <param name="srcPath">The source path of the file to copy from the source filesystem</param>
        /// <param name="destPath">The destination path of the file in the destination filesystem</param>
        /// <param name="overwrite"><c>true</c> to overwrite an existing destination file</param>
        public static void CopyFileCross(UPath srcPath, UPath destPath, bool overwrite)
        {
            FileSystem.Instance.CopyFile(srcPath, destPath, overwrite);
        }

        /// <summary>
        ///     Moves a file between two filesystems.
        /// </summary>
        /// <param name="fs">The source filesystem</param>
        /// <param name="destFileSystem">The destination filesystem</param>
        /// <param name="srcPath">The source path of the file to move from the source filesystem</param>
        /// <param name="destPath">The destination path of the file in the destination filesystem</param>
        public static void MoveFileCross(UPath srcPath, UPath destPath)
        {
            FileSystem.Instance.MoveFile(srcPath, destPath);
        }

        /// <summary>
        ///     Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
        public static byte[] ReadAllBytes(UPath path)
        {
            var memstream = new MemoryStream();
            using (var stream = FileSystem.Instance.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                stream.CopyTo(memstream);
            }
            return memstream.ToArray();
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        /// <remarks>
        ///     This method attempts to automatically detect the encoding of a file based on the presence of byte order marks.
        ///     Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected.
        /// </remarks>
        public static string ReadAllText(UPath path)
        {
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for reading.</param>
        /// <param name="encoding">The encoding to use to decode the text from <paramref name="path" />.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static string ReadAllText(UPath path, Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        ///     Creates a new file, writes the specified byte array to the file, and then closes the file.
        ///     If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for writing.</param>
        /// <param name="content">The content.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a byte array and a file path, this method opens the specified file, writes the
        ///     contents of the byte array to the file, and then closes the file.
        /// </remarks>
        public static void WriteAllBytes(UPath path, byte[] content)
        {
            Assert.ThrowIfArgumentNull(content, nameof(content));
            using (var stream = FileSystem.Instance.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                stream.Write(content, 0, content.Length);
            }
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for reading.</param>
        /// <returns>An array of strings containing all lines of the file.</returns>
        public static string[] ReadAllLines(UPath path)
        {
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            {
                using (var reader = new StreamReader(stream))
                {
                    var lines = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    return lines.ToArray();
                }
            }
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for reading.</param>
        /// <param name="encoding">The encoding to use to decode the text from <paramref name="path" />.</param>
        /// <remarks>
        ///     This method attempts to automatically detect the encoding of a file based on the presence of byte order marks.
        ///     Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected.
        /// </remarks>
        /// <returns>An array of strings containing all lines of the file.</returns>
        public static string[] ReadAllLines(UPath path, Encoding encoding)
        {
            Assert.ThrowIfArgumentNull(encoding, nameof(encoding));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    var lines = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    return lines.ToArray();
                }
            }
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file, and then closes the file.
        ///     If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for writing.</param>
        /// <param name="content">The content.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the GetPreamble method will return an
        ///     empty byte array.
        ///     If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file,
        ///     use the <see cref="WriteAllText(Zio.IFileSystem,Zio.UPath,string, Encoding)" /> method overload with UTF8 encoding.
        /// </remarks>
        public static void WriteAllText(UPath path, string content)
        {
            Assert.ThrowIfArgumentNull(content, nameof(content));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file using the specified encoding, and then
        ///     closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for writing.</param>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding to use to decode the text from <paramref name="path" />. </param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, writes the string to the file using the
        ///     specified encoding, and then closes the file.
        ///     The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        /// </remarks>
        public static void WriteAllText(UPath path, string content, Encoding encoding)
        {
            Assert.ThrowIfArgumentNull(content, nameof(content));
            Assert.ThrowIfArgumentNull(encoding, nameof(encoding));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            {
                using (var writer = new StreamWriter(stream, encoding))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        ///     Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist,
        ///     this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for appending.</param>
        /// <param name="content">The content to append.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, appends the string to the end of the file,
        ///     and then closes the file. The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        ///     The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the
        ///     path parameter must contain existing directories.
        /// </remarks>
        public static void AppendAllText(UPath path, string content)
        {
            Assert.ThrowIfArgumentNull(content, nameof(content));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        ///     Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="fs">The filesystem.</param>
        /// <param name="path">The path of the file to open for appending.</param>
        /// <param name="content">The content to append.</param>
        /// <param name="encoding">The encoding to use to encode the text from <paramref name="path" />.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, appends the string to the end of the file,
        ///     and then closes the file. The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        ///     The method creates the file if it doesn’t exist, but it doesn't create new directories. Therefore, the value of the
        ///     path parameter must contain existing directories.
        /// </remarks>
        public static void AppendAllText(UPath path, string content, Encoding encoding)
        {
            Assert.ThrowIfArgumentNull(content, nameof(content));
            Assert.ThrowIfArgumentNull(encoding, nameof(encoding));
            var stream = FileSystem.Instance.OpenFile(path, FileMode.Append, FileAccess.Write, FileShare.Read);
            {
                using (var writer = new StreamWriter(stream, encoding))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Creates or overwrites a file in the specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path and name of the file to create.</param>
        /// <returns>A stream that provides read/write access to the file specified in path.</returns>
        public static Stream CreateFile(UPath path)
        {
            path.AssertAbsolute();
            return FileSystem.Instance.OpenFile(path, FileMode.Create, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Returns an enumerable collection of directory names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for directories.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateDirectories(UPath path)
        {
            return EnumerateDirectories(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of directory names that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateDirectories(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

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
        public static IEnumerable<UPath> EnumerateDirectories(UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            Assert.ThrowIfArgumentNull(searchOption, nameof(searchOption));
            foreach (var subPath in FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.Directory))
                yield return subPath;
        }

        /// <summary>
        /// Returns an enumerable collection of file names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateFiles(UPath path)
        {
            return FileSystemExtensions.EnumerateFiles(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of file names that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumerateFiles(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
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
        public static IEnumerable<UPath> EnumerateFiles(UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.File))
            {
                yield return subPath;
            }
        }

        /// <summary>
        /// Returns an enumerable collection of file or directory names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files or directories.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files and directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumeratePaths(UPath path)
        {
            return EnumeratePaths(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of file or directory names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files or directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files and directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumeratePaths(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumeratePaths(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Returns an enumerable collection of file or directory names in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files or directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of the full names (including paths) for the files and directories in the directory specified by path.</returns>
        public static IEnumerable<UPath> EnumeratePaths(UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            Assert.ThrowIfArgumentNull(searchOption, nameof(searchOption));
            return FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, SearchTarget.Both);
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <returns>An enumerable collection of <see cref="FileEntry"/> from the specified path.</returns>
        public static IEnumerable<FileEntry> EnumerateFileEntries(UPath path)
        {
            return EnumerateFileEntries(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of <see cref="FileEntry"/> from the specified path.</returns>
        public static IEnumerable<FileEntry> EnumerateFileEntries(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumerateFileEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
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
        public static IEnumerable<FileEntry> EnumerateFileEntries(UPath path, string searchPattern, SearchOption searchOption)
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
        /// <returns>An enumerable collection of <see cref="DirectoryEntry"/> from the specified path.</returns>
        public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(UPath path)
        {
            return EnumerateDirectoryEntries(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="DirectoryEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of <see cref="DirectoryEntry"/> from the specified path.</returns>
        public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumerateDirectoryEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
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
        public static IEnumerable<DirectoryEntry> EnumerateDirectoryEntries(UPath path, string searchPattern, SearchOption searchOption)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in FileSystemExtensions.EnumerateDirectories(path, searchPattern, searchOption))
            {
                yield return new DirectoryEntry(subPath);
            }
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files and directories.</param>
        /// <returns>An enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.</returns>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(UPath path)
        {
            return EnumerateFileSystemEntries(path, "*");
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files and directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <returns>An enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.</returns>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(UPath path, string searchPattern)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            return EnumerateFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path of the directory to look for files and directories.</param>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <param name="searchTarget">The search target either <see cref="SearchTarget.Both"/> or only <see cref="SearchTarget.Directory"/> or <see cref="SearchTarget.File"/>. Default is <see cref="SearchTarget.Both"/></param>
        /// <returns>An enumerable collection of <see cref="FileSystemEntry"/> that match a search pattern in a specified path.</returns>
        public static IEnumerable<FileSystemEntry> EnumerateFileSystemEntries(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget = SearchTarget.Both)
        {
            Assert.ThrowIfArgumentNull(searchPattern, nameof(searchPattern));
            foreach (var subPath in FileSystem.Instance.EnumeratePaths(path, searchPattern, searchOption, searchTarget))
            {
                yield return subPath.DirectoryExists() ? (FileSystemEntry) new DirectoryEntry(subPath) : new FileEntry(subPath);
            }
        }

        /// <summary>
        /// Gets a <see cref="FileSystemEntry"/> for the specified path. If the file or directory does not exist, throws a <see cref="FileNotFoundException"/>
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The file or directory path.</param>
        /// <returns>A new <see cref="FileSystemEntry"/> from the specified path.</returns>
        public static FileSystemEntry GetFileSystemEntry(UPath path)
        {
            var fileExists = FileSystem.Instance.FileExists(path);
            if (fileExists)
            {
                return new FileEntry(path);
            }
            var directoryExists = FileSystem.Instance.DirectoryExists(path);
            if (directoryExists)
            {
                return new DirectoryEntry(path);
            }

            throw Error.NewEntryDoesntExists(path);
        }

        /// <summary>
        /// Tries to get a <see cref="FileSystemEntry"/> for the specified path. If the file or directory does not exist, returns null.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The file or directory path.</param>
        /// <returns>A new <see cref="FileSystemEntry"/> from the specified path.</returns>
        public static FileSystemEntry TryGetFileSystemEntry(UPath path)
        {
            var fileExists = FileSystem.Instance.FileExists(path);
            if (fileExists)
            {
                return new FileEntry(path);
            }
            var directoryExists = FileSystem.Instance.DirectoryExists(path);
            if (directoryExists)
            {
                return new DirectoryEntry(path);
            }

            return null;
        }

        /// <summary>
        /// Gets a <see cref="FileEntry"/> for the specified path. If the file does not exist, throws a <see cref="FileNotFoundException"/>
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>A new <see cref="FileEntry"/> from the specified path.</returns>
        public static FileEntry GetFileEntry(UPath filePath)
        {
            if (!FileSystem.Instance.FileExists(filePath))
            {
                throw Error.NewFileNotFoundException(filePath);
            }
            return new FileEntry(filePath);
        }

        /// <summary>
        /// Gets a <see cref="DirectoryEntry"/> for the specified path. If the file does not exist, throws a <see cref="DirectoryNotFoundException"/>
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>A new <see cref="DirectoryEntry"/> from the specified path.</returns>
        public static DirectoryEntry GetDirectoryEntry(UPath directoryPath)
        {
            if (!FileSystem.Instance.DirectoryExists(directoryPath))
            {
                throw Error.NewDirectoryNotFoundException(directoryPath);
            }
            return new DirectoryEntry(directoryPath);
        }
    }
}