// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using McMorph.Results;

namespace McMorph.Files
{
    /// <summary>
    /// Provides default arguments safety checking and redirecting to safe implementation.
    /// Implements also the <see cref="IDisposable"/> pattern.
    /// </summary>
    public class FileSystem
    {
        public static readonly FileSystem Instance = new FileSystem();

        private FileSystem()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileSystem"/> class.
        /// </summary>
        ~FileSystem()
        {
            DisposeInternal(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <c>true</c> if this instance if being disposed.
        /// </summary>
        protected bool IsDisposing { get; private set; }

        /// <summary>
        /// <c>true</c> if this instance if being disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        // ----------------------------------------------
        // Directory API
        // ----------------------------------------------

        /// <summary>
        /// Creates all directories and subdirectories in the specified path unless they already exist.
        /// </summary>
        /// <param name="path">The directory to create.</param>
        public void CreateDirectory(UPath path)
        {
            AssertNotDisposed();
            if (path == UPath.Root)
            {
                throw new UnauthorizedAccessException("Cannot create root directory `/`");
            }
            Directory.CreateDirectory(path.FullName);
        }


        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns><c>true</c> if the given path refers to an existing directory on disk, <c>false</c> otherwise.</returns>
        public bool DirectoryExists(UPath path)
        {
            AssertNotDisposed();

            // With FileExists, case where a null path is allowed
            if (path.IsNull)
            {
                return false;
            }
            return Directory.Exists(ValidatePath(path).FullName);
        }


        /// <summary>
        /// Moves a directory and its contents to a new location.
        /// </summary>
        /// <param name="srcPath">The path of the directory to move.</param>
        /// <param name="destPath">The path to the new location for <paramref name="srcPath"/></param>
        public void MoveDirectory(UPath srcPath, UPath destPath)
        {
            AssertNotDisposed();
            if (srcPath == UPath.Root)
            {
                throw new UnauthorizedAccessException("Cannot move from the source root directory `/`");
            }
            if (destPath == UPath.Root)
            {
                throw new UnauthorizedAccessException("Cannot move to the root directory `/`");
            }

            if (srcPath == destPath)
            {
                throw new IOException($"The source and destination path are the same `{srcPath}`");
            }

            var systemSrcPath = ValidatePath(srcPath, nameof(srcPath)).FullName;
            var systemDestPath = ValidatePath(destPath, nameof(destPath)).FullName;

            // If the souce path is a file
            var fileInfo = new FileInfo(systemSrcPath);
            if (fileInfo.Exists)
            {
                throw new IOException($"The source `{srcPath}` is not a directory");
            }

            Directory.Move(systemSrcPath, systemDestPath);
        }


        /// <summary>
        /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory. 
        /// </summary>
        /// <param name="path">The path of the directory to remove.</param>
        /// <param name="isRecursive"><c>true</c> to remove directories, subdirectories, and files in path; otherwise, <c>false</c>.</param>
        public void DeleteDirectory(UPath path, bool isRecursive)
        {
            AssertNotDisposed();
            if (path == UPath.Root)
            {
                throw new UnauthorizedAccessException("Cannot delete root directory `/`");
            }

            Directory.Delete(ValidatePath(path).FullName, isRecursive);
        }


        // ----------------------------------------------
        // File API
        // ----------------------------------------------

        /// <summary>
        /// Copies an existing file to a new file. Overwriting a file of the same name is allowed.
        /// </summary>
        /// <param name="srcPath">The path of the file to copy.</param>
        /// <param name="destPath">The path of the destination file. This cannot be a directory.</param>
        /// <param name="overwrite"><c>true</c> if the destination file can be overwritten; otherwise, <c>false</c>.</param>
        public void CopyFile(UPath srcPath, UPath destPath, bool overwrite)
        {
            AssertNotDisposed();
            File.Copy(ValidatePath(srcPath, nameof(srcPath)).FullName, ValidatePath(destPath, nameof(destPath)).FullName, overwrite);
        }

        /// <summary>
        /// Gets the size, in bytes, of a file.
        /// </summary>
        /// <param name="path">The path of a file.</param>
        /// <returns>The size, in bytes, of the file</returns>
        public long GetFileLength(UPath path)
        {
            AssertNotDisposed();
            return new FileInfo(ValidatePath(path).FullName).Length;
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if the caller has the required permissions and path contains the name of an existing file; 
        /// otherwise, <c>false</c>. This method also returns false if path is null, an invalid path, or a zero-length string. 
        /// If the caller does not have sufficient permissions to read the specified file, 
        /// no exception is thrown and the method returns false regardless of the existence of path.</returns>
        public bool FileExists(UPath path)
        {
            AssertNotDisposed();

            // Only case where a null path is allowed
            if (path.IsNull)
            {
                return false;
            }
            return File.Exists(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="srcPath">The path of the file to move.</param>
        /// <param name="destPath">The new path and name for the file.</param>
        public void MoveFile(UPath srcPath, UPath destPath)
        {
            AssertNotDisposed();
            File.Move(ValidatePath(srcPath, nameof(srcPath)).FullName, ValidatePath(destPath, nameof(destPath)).FullName);
        }

        /// <summary>
        /// Deletes the specified file. 
        /// </summary>
        /// <param name="path">The path of the file to be deleted.</param>
        public void DeleteFile(UPath path)
        {
            AssertNotDisposed();
            File.Delete(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Opens a file <see cref="Stream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <param name="path">The path to the file to open.</param>
        /// <param name="mode">A <see cref="FileMode"/> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
        /// <param name="access">A <see cref="FileAccess"/> value that specifies the operations that can be performed on the file.</param>
        /// <param name="share">A <see cref="FileShare"/> value specifying the type of access other threads have to the file.</param>
        /// <returns>A file <see cref="Stream"/> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
        public Stream OpenFile(UPath path, FileMode mode, FileAccess access, FileShare share = FileShare.None)
        {
            AssertNotDisposed();
            return File.Open(ValidatePath(path).FullName, mode, access, share);
        }

        // ----------------------------------------------
        // Metadata API
        // ----------------------------------------------

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of the file or directory on the path.
        /// </summary>
        /// <param name="path">The path to the file or directory.</param>
        /// <returns>The <see cref="FileAttributes"/> of the file or directory on the path.</returns>
        public FileAttributes GetAttributes(UPath path)
        {
            AssertNotDisposed();
            return File.GetAttributes(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Sets the specified <see cref="FileAttributes"/> of the file or directory on the specified path.
        /// </summary>
        /// <param name="path">The path to the file or directory.</param>
        /// <param name="attributes">A bitwise combination of the enumeration values.</param>
        public void SetAttributes(UPath path, FileAttributes attributes)
        {
            AssertNotDisposed();
            File.SetAttributes(ValidatePath(path).FullName, attributes);
        }

        /// <summary>
        /// Returns the creation date and time of the specified file or directory.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to obtain creation date and time information.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
        public DateTime GetCreationTime(UPath path)
        {
            AssertNotDisposed();
            return File.GetCreationTime(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Sets the date and time the file was created.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to set the creation date and time.</param>
        /// <param name="time">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
        public void SetCreationTime(UPath path, DateTime time)
        {
            AssertNotDisposed();
            File.SetCreationTime(ValidatePath(path).FullName, time);
        }

        /// <summary>
        /// Returns the last access date and time of the specified file or directory.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to obtain creation date and time information.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the last access date and time for the specified file or directory. This value is expressed in local time.</returns>
        public DateTime GetLastAccessTime(UPath path)
        {
            AssertNotDisposed();
            return File.GetLastAccessTime(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Sets the date and time the file was last accessed.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to set the last access date and time.</param>
        /// <param name="time">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        public void SetLastAccessTime(UPath path, DateTime time)
        {
            AssertNotDisposed();
            File.SetLastAccessTime(ValidatePath(path).FullName, time);
        }

        /// <summary>
        /// Returns the last write date and time of the specified file or directory.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to obtain creation date and time information.</param>
        /// <returns>A <see cref="DateTime"/> structure set to the last write date and time for the specified file or directory. This value is expressed in local time.</returns>
        public DateTime GetLastWriteTime(UPath path)
        {
            AssertNotDisposed();
            return File.GetLastWriteTime(ValidatePath(path).FullName);
        }

        /// <summary>
        /// Sets the date and time that the specified file was last written to.
        /// </summary>
        /// <param name="path">The path to a file or directory for which to set the last write date and time.</param>
        /// <param name="time">A <see cref="DateTime"/> containing the value to set for the last write date and time of path. This value is expressed in local time.</param>
        public void SetLastWriteTime(UPath path, DateTime time)
        {
            AssertNotDisposed();
            File.SetLastWriteTime(ValidatePath(path).FullName, time);
        }

        // ----------------------------------------------
        // Search API
        // ----------------------------------------------

        /// <summary>
        /// Returns an enumerable collection of file names and/or directory names that match a search pattern in a specified path, and optionally searches subdirectories.
        /// </summary>
        /// <param name="path">The path to the directory to search.</param>
        /// <param name="searchPattern">The search string to match against file-system entries in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters (see Remarks), but doesn't support regular expressions.</param>
        /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
        /// <param name="searchTarget">The search target either <see cref="SearchTarget.Both"/> or only <see cref="SearchTarget.Directory"/> or <see cref="SearchTarget.File"/>.</param>
        /// <returns>An enumerable collection of file-system paths in the directory specified by path and that match the specified search pattern, option and target.</returns>
        public IEnumerable<UPath> EnumeratePaths(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
        {
            AssertNotDisposed();
            Assert.ThrowIfArgumentNull(searchOption, nameof(searchOption));

            IEnumerable<string> results;
            switch (searchTarget)
            {
                case SearchTarget.File:
                    results = Directory.EnumerateFiles(ValidatePath(path).FullName, searchPattern, searchOption);
                    break;

                case SearchTarget.Directory:
                    results = Directory.EnumerateDirectories(ValidatePath(path).FullName, searchPattern, searchOption);
                    break;

                case SearchTarget.Both:
                    results = Directory.EnumerateFileSystemEntries(ValidatePath(path).FullName, searchPattern, searchOption);
                    break;
                
                default:
                    yield break;
            }

            foreach (var subPath in results)
            {
                yield return ConvertPathFromInternal(subPath);
            }

        }

        // ----------------------------------------------
        // Path API
        // ----------------------------------------------

        /// <summary>
        /// Converts the specified path to the underlying path used by this <see cref="IFileSystem"/>. In case of a <see cref="McMorph.Files.FileSystems.PhysicalFileSystem"/>, it 
        /// would represent the actual path on the disk.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The converted system path according to the specified path.</returns>
        public string ConvertPathToInternal(UPath path)
        {
            AssertNotDisposed();
            return ValidatePath(path).FullName;
        }

        /// <summary>
        /// Converts the specified system path to a <see cref="IFileSystem"/> path.
        /// </summary>
        /// <param name="systemPath">The system path.</param>
        /// <returns>The converted path according to the system path.</returns>
        public UPath ConvertPathFromInternal(string systemPath)
        {
            AssertNotDisposed();
            Assert.ThrowIfArgumentNull(systemPath, nameof(systemPath));
            return systemPath;
        }

        /// <summary>
        /// Validates the specified path (Check that it is absolute by default)
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="argumentName">The name.</param>
        /// <param name="allowNull">if set to <c>true</c> the path is allowed to be null. <c>false</c> otherwise.</param>  
        /// <returns>The path validated</returns>
        protected UPath ValidatePath(UPath path, string argumentName = "path", bool allowNull = false)
        {
            if (allowNull && path.IsNull)
            {
                return path;
            }
            path.AssertAbsolute(argumentName);

            if (path.FullName.IndexOf(':') >= 0)
            {
                throw new NotSupportedException($"The path `{path}` cannot contain the `:` character");
            }
            return path;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        private void AssertNotDisposed()
        {
            if (IsDisposing || IsDisposed)
            {
                throw new ObjectDisposedException($"This instance `{GetType()}` is already disposed.");
            }
        }

        private void DisposeInternal(bool disposing)
        {
            AssertNotDisposed();
            IsDisposing = true;
            Dispose(disposing);
            IsDisposed = true;
        }
    }
}