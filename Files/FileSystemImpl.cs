// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace McMorph.Files
{
    /// <summary>
    /// Provides a <see cref="IFileSystem"/> for the physical filesystem.
    /// </summary>
    public class FileSystemImpl : FileSystem
    {
        // ----------------------------------------------
        // Directory API
        // ----------------------------------------------

        /// <inheritdoc />
        protected override void CreateDirectoryImpl(UPath path)
        {
            Directory.CreateDirectory(path.FullName);
        }

        /// <inheritdoc />
        protected override bool DirectoryExistsImpl(UPath path)
        {
            return Directory.Exists(path.FullName);
        }

        /// <inheritdoc />
        protected override void MoveDirectoryImpl(UPath srcPath, UPath destPath)
        {
            var systemSrcPath = srcPath.FullName;
            var systemDestPath = destPath.FullName;

            // If the souce path is a file
            var fileInfo = new FileInfo(systemSrcPath);
            if (fileInfo.Exists)
            {
                throw new IOException($"The source `{srcPath}` is not a directory");
            }

            Directory.Move(systemSrcPath, systemDestPath);
        }

        /// <inheritdoc />
        protected override void DeleteDirectoryImpl(UPath path, bool isRecursive)
        {
            Directory.Delete(path.FullName, isRecursive);
        }

        // ----------------------------------------------
        // File API
        // ----------------------------------------------

        /// <inheritdoc />
        protected override void CopyFileImpl(UPath srcPath, UPath destPath, bool overwrite)
        {
            File.Copy(srcPath.FullName, destPath.FullName, overwrite);
        }

        /// <inheritdoc />
        protected override long GetFileLengthImpl(UPath path)
        {
            return new FileInfo(path.FullName).Length;
        }

        /// <inheritdoc />
        protected override bool FileExistsImpl(UPath path)
        {
            return File.Exists(path.FullName);
        }

        /// <inheritdoc />
        protected override void MoveFileImpl(UPath srcPath, UPath destPath)
        {
            File.Move(srcPath.FullName, destPath.FullName);
        }

        /// <inheritdoc />
        protected override void DeleteFileImpl(UPath path)
        {
            File.Delete(path.FullName);
        }

        /// <inheritdoc />
        protected override Stream OpenFileImpl(UPath path, FileMode mode, FileAccess access,
            FileShare share = FileShare.None)
        {
            return File.Open(path.FullName, mode, access, share);
        }

        /// <inheritdoc />
        protected override FileAttributes GetAttributesImpl(UPath path)
        {
            return File.GetAttributes(path.FullName);
        }

        // ----------------------------------------------
        // Metadata API
        // ----------------------------------------------

        /// <inheritdoc />
        protected override void SetAttributesImpl(UPath path, FileAttributes attributes)
        {
            File.SetAttributes(path.FullName, attributes);
        }

        /// <inheritdoc />
        protected override DateTime GetCreationTimeImpl(UPath path)
        {
            return File.GetCreationTime(path.FullName);
        }

        /// <inheritdoc />
        protected override void SetCreationTimeImpl(UPath path, DateTime time)
        {
            File.SetCreationTime(path.FullName, time);
        }

        /// <inheritdoc />
        protected override DateTime GetLastAccessTimeImpl(UPath path)
        {
            return File.GetLastAccessTime(path.FullName);
        }

        /// <inheritdoc />
        protected override void SetLastAccessTimeImpl(UPath path, DateTime time)
        {
            File.SetLastAccessTime(path.FullName, time);
        }

        /// <inheritdoc />
        protected override DateTime GetLastWriteTimeImpl(UPath path)
        {
            return File.GetLastWriteTime(path.FullName);
        }

        /// <inheritdoc />
        protected override void SetLastWriteTimeImpl(UPath path, DateTime time)
        {
            File.SetLastWriteTime(path.FullName, time);
        }

        // ----------------------------------------------
        // Search API
        // ----------------------------------------------

        /// <inheritdoc />
        protected override IEnumerable<UPath> EnumeratePathsImpl(UPath path, string searchPattern, SearchOption searchOption, SearchTarget searchTarget)
        {
            IEnumerable<string> results;
            switch (searchTarget)
            {
                case SearchTarget.File:
                    results = Directory.EnumerateFiles(path.FullName, searchPattern, searchOption);
                    break;

                case SearchTarget.Directory:
                    results = Directory.EnumerateDirectories(path.FullName, searchPattern, searchOption);
                    break;

                case SearchTarget.Both:
                    results = Directory.EnumerateFileSystemEntries(path.FullName, searchPattern, searchOption);
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

        /// <inheritdoc />
        protected override string ConvertPathToInternalImpl(UPath path)
        {
            return path.FullName;
        }

        /// <inheritdoc />
        protected override UPath ConvertPathFromInternalImpl(string innerPath)
        {
            return innerPath;
        }
    }
}