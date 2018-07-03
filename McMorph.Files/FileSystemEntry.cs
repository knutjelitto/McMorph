// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.
using System;
using System.IO;

namespace McMorph.Files
{
    /// <summary>
    /// Similar to <see cref="FileSystemInfo"/> but to use with <see cref="FileSystem"/>, provides the base class
    /// for both <see cref="FileEntry"/> and <see cref="DirectoryEntry"/> objects.
    /// </summary>
    public abstract class FileSystemEntry : IEquatable<FileSystemEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemEntry"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path to the file or directory.</param>
        /// <exception cref="System.ArgumentNullException">fileSystem</exception>
        protected FileSystemEntry(UPath path)
        {
            path.AssertAbsolute();
            Path = path;
        }
        
        protected FileSystem FS => FileSystem.Instance;


        /// <summary>
        /// Gets the path of this entry.
        /// </summary>
        public UPath Path { get; }

        /// <summary>
        /// Gets the full path of the directory or file.
        /// </summary>
        public string FullName => Path.FullName;

        /// <summary>
        /// Gets the name of the file or directory (with its extension).
        /// </summary>
        public string Name => Path.GetName();


        /// <summary>
        /// Gets a value indicating whether this file or directory exists.
        /// </summary>
        /// <value><c>true</c> if this file or directory exists; otherwise, <c>false</c>.</value>
        public abstract bool Exists { get; }

        /// <summary>Gets an instance of the parent directory.</summary>
        /// <returns>A <see cref="DirectoryEntry" /> object representing the parent directory of this file.</returns>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid, such as being on an unmapped
        ///     drive.
        /// </exception>
        public DirectoryEntry Parent => Path == UPath.Root ? null : new DirectoryEntry(Path / "..");

        /// <summary>
        /// Deletes a file or directory.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Returns the <see cref="FullName"/> of this instance.
        /// </summary>
        /// <returns>The <see cref="FullName"/> of this instance.</returns>
        public override string ToString()
        {
            return Path.FullName;
        }

        /// <inheritdoc />
        public bool Equals(FileSystemEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Path.Equals(other.Path);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileSystemEntry) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public static bool operator ==(FileSystemEntry left, FileSystemEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileSystemEntry left, FileSystemEntry right)
        {
            return !Equals(left, right);
        }
    }
}