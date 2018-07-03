// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.IO;
using System.Text;

using McMorph.Tools;

namespace McMorph.Files
{
    /// <summary>
    /// Similar to <see cref="FileInfo"/> but to use with <see cref="FileSystem"/>, provides properties and instance methods 
    /// for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects. 
    /// Note that unlike <see cref="FileInfo"/>, this class doesn't cache any data.
    /// </summary>
    public class FileEntry : FileSystemEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileEntry"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The file path.</param>
        public FileEntry(UPath path) : base(path)
        {
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <returns>An array of strings containing all lines of the file.</returns>
        public string[] ReadAllLines()
        {
            return FileSystemExtensions.ReadAllLines(Path);
        }

        /// <summary>
        ///     Creates a new file, writes the specified byte array to the file, and then closes the file.
        ///     If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a byte array and a file path, this method opens the specified file, writes the
        ///     contents of the byte array to the file, and then closes the file.
        /// </remarks>
        public void WriteAllBytes(byte[] content)
        {
            FileSystemExtensions.WriteAllBytes(Path, content);
        }

        /// <inheritdoc />
        public override bool Exists => FS.FileExists(Path);

        /// <inheritdoc />
        public override void Delete()
        {
            FS.DeleteFile(Path);
        }
    }
}