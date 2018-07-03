// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;

namespace McMorph.Files
{

    /// <summary>
    /// Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. 
    /// </summary>
    public class DirectoryEntry : FileSystemEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryEntry"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The directory path.</param>
        public DirectoryEntry(UPath path) : base(path)
        {
        }

        /// <summary>Creates a directory.</summary>
        /// <exception cref="T:System.IO.IOException">The directory cannot be created. </exception>
        public void Create()
        {
            FS.CreateDirectory(Path);
        }

        /// <summary>Deletes this instance of a <see cref="T:System.IO.DirectoryInfo" />, specifying whether to delete subdirectories and files.</summary>
        /// <param name="recursive">true to delete this directory, its subdirectories, and all files; otherwise, false. </param>
        /// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this <see cref="T:System.IO.DirectoryInfo" /> object does not exist or could not be found.</exception>
        /// <exception cref="T:System.IO.IOException">The directory is read-only.-or- The directory contains one or more files or subdirectories and <paramref name="recursive" /> is false.-or-The directory is the application's current working directory. -or-There is an open handle on the directory or on one of its files, and the operating system is Windows XP or earlier. This open handle can result from enumerating directories and files. For more information, see How to: Enumerate Directories and Files.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void Delete(bool recursive)
        {
            FS.DeleteDirectory(Path, recursive);
        }

        /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option. </summary>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of directories.</returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public IEnumerable<DirectoryEntry> EnumerateDirectories(string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Path.EnumerateDirectoryEntries(searchPattern, searchOption);
        }

        /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
        /// <param name="searchPattern">The search string to match against the names of directories in path. This parameter can contain a combination 
        /// of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory 
        /// or should include all subdirectories.
        /// The default value is TopDirectoryOnly.</param>
        /// <returns>An enumerable collection of files.</returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the <see cref="T:System.IO.DirectoryInfo" /> object is invalid (for example, it is on an unmapped drive). </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public IEnumerable<FileEntry> EnumerateFiles(string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return Path.EnumerateFileEntries(searchPattern, searchOption);
        }

        /// <summary>Moves a <see cref="T:System.IO.DirectoryInfo" /> instance and its contents to a new path.</summary>
        /// <param name="destDirName">The name and path to which to move this directory. The destination cannot be another disk volume or a directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="destDirName" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="destDirName" /> is an empty string (''"). </exception>
        /// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume. -or-<paramref name="destDirName" /> already exists.-or-You are not authorized to access this path.-or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The destination directory cannot be found.</exception>
        public void MoveTo(UPath destDirName)
        {
            FS.MoveDirectory(Path, destDirName);
        }

        /// <inheritdoc />
        public override bool Exists => FS.DirectoryExists(Path);

        /// <inheritdoc />
        public override void Delete()
        {
            Delete(true);
        }
    }
}