// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.IO;
using System.Text;

using McMorph.Results;

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

        /// <summary>Gets an instance of the parent directory.</summary>
        /// <returns>A <see cref="DirectoryEntry" /> object representing the parent directory of this file.</returns>
        /// <exception cref="DirectoryNotFoundException">
        ///     The specified path is invalid, such as being on an unmapped
        ///     drive.
        /// </exception>
        public DirectoryEntry Directory => Parent;

        /// <summary>Gets or sets a value that determines if the current file is read only.</summary>
        /// <returns>true if the current file is read only; otherwise, false.</returns>
        /// <exception cref="T:System.IO.FileNotFoundException">
        ///     The file described by the current
        ///     <see cref="T:System.IO.FileInfo" /> object could not be found.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     This operation is not supported on the current platform.-or- The
        ///     caller does not have the required permission.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     The user does not have write permission, but attempted to set this
        ///     property to false.
        /// </exception>
        public bool IsReadOnly => (FS.GetAttributes(Path) & FileAttributes.ReadOnly) != 0;

        /// <summary>Gets the size, in bytes, of the current file.</summary>
        /// <returns>The size of the current file in bytes.</returns>
        /// <exception cref="T:System.IO.IOException">
        ///     <see cref="M:System.IO.FileSystemInfo.Refresh" /> cannot update the state of the file or directory.
        /// </exception>
        /// <exception cref="T:System.IO.FileNotFoundException">
        ///     The file does not exist.-or- The Length property is called for a
        ///     directory.
        /// </exception>
        public long Length => FS.GetFileLength(Path);

        /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
        /// <returns>
        ///     A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is true. If the file exists
        ///     and <paramref name="overwrite" /> is false, an <see cref="T:System.IO.IOException" /> is thrown.
        /// </returns>
        /// <param name="destFileName">The name of the new file to copy to. </param>
        /// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false. </param>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        ///     An error occurs, or the destination file already exists and
        ///     <paramref name="overwrite" /> is false.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="destFileName" /> is null.
        /// </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The directory specified in <paramref name="destFileName" />
        ///     does not exist.
        /// </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     A directory path is passed in, or the file is being moved to a
        ///     different drive.
        /// </exception>
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined
        ///     maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names
        ///     must be less than 260 characters.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="destFileName" /> contains a colon (:) in the middle of the string.
        /// </exception>
        public FileEntry CopyTo(UPath destFileName, bool overwrite)
        {
            FS.CopyFile(Path, destFileName, overwrite);
            return new FileEntry(destFileName);
        }

        /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
        /// <returns>
        ///     A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is true. If the file exists
        ///     and <paramref name="overwrite" /> is false, an <see cref="T:System.IO.IOException" /> is thrown.
        /// </returns>
        /// <param name="destFile">The file entry to copy to. </param>
        /// <param name="overwrite">true to allow an existing file to be overwritten; otherwise, false. </param>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="destFile" /> is null.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        ///     An error occurs, or the destination file already exists and
        ///     <paramref name="overwrite" /> is false.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The directory specified in <paramref name="destFile" />
        ///     does not exist.
        /// </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     A directory path is passed in, or the file is being moved to a
        ///     different drive.
        /// </exception>
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined
        ///     maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names
        ///     must be less than 260 characters.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="destFile" /> contains a colon (:) in the middle of the string.
        /// </exception>
        public FileEntry CopyTo(FileEntry destFile, bool overwrite)
        {
            Assert.ThrowIfArgumentNull(destFile, nameof(destFile));
            FileSystemExtensions.CopyFileCross(Path, destFile.Path, overwrite);
            return destFile;
        }

        /// <summary>Creates a file.</summary>
        /// <returns>A new file.</returns>
        public Stream Create()
        {
            return FileSystemExtensions.CreateFile(Path);
        }

        /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
        /// <param name="destFileName">The path to move the file to, which can specify a different file name. </param>
        /// <exception cref="T:System.IO.IOException">
        ///     An I/O error occurs, such as the destination file already exists or the
        ///     destination device is not ready.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="destFileName" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="destFileName" /> is empty, contains only white spaces, or contains invalid characters.
        /// </exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     <paramref name="destFileName" /> is read-only or is a directory.
        /// </exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The specified path is invalid, such as being on an unmapped
        ///     drive.
        /// </exception>
        /// <exception cref="T:System.IO.PathTooLongException">
        ///     The specified path, file name, or both exceed the system-defined
        ///     maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names
        ///     must be less than 260 characters.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///     <paramref name="destFileName" /> contains a colon (:) in the middle of the string.
        /// </exception>
        public void MoveTo(UPath destFileName)
        {
            FS.MoveFile(Path, destFileName);
        }

        /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
        /// <returns>A <see cref="T:System.IO.FileStream" /> object opened with the specified mode, access, and sharing options.</returns>
        /// <param name="mode">
        ///     A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in
        ///     which to open the file.
        /// </param>
        /// <param name="access">
        ///     A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with Read,
        ///     Write, or ReadWrite file access.
        /// </param>
        /// <param name="share">
        ///     A <see cref="T:System.IO.FileShare" /> constant specifying the type of access other FileStream
        ///     objects have to this file.
        /// </param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found. </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///     The path is read-only or is a directory.
        /// </exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">
        ///     The specified path is invalid, such as being on an unmapped
        ///     drive.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">The file is already open. </exception>
        public Stream Open(FileMode mode, FileAccess access, FileShare share = FileShare.None)
        {
            return FS.OpenFile(Path, mode, access, share);
        }


        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <returns>A string containing all lines of the file.</returns>
        /// <remarks>
        ///     This method attempts to automatically detect the encoding of a file based on the presence of byte order marks.
        ///     Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected.
        /// </remarks>
        public string ReadAllText()
        {
            return FileSystemExtensions.ReadAllText(Path);
        }

        /// <summary>
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding to use to decode the text from <see cref="Path" />.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public string ReadAllText(Encoding encoding)
        {
            return FileSystemExtensions.ReadAllText(Path, encoding);
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file, and then closes the file.
        ///     If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the GetPreamble method will return an
        ///     empty byte array.
        ///     If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file,
        ///     use the <see cref="WriteAllText(string, Encoding)" /> method overload with UTF8 encoding.
        /// </remarks>
        public void WriteAllText(string content)
        {
            FileSystemExtensions.WriteAllText(Path, content);
        }

        /// <summary>
        ///     Creates a new file, writes the specified string to the file using the specified encoding, and then
        ///     closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="encoding">The encoding to use to decode the text from <paramref name="path" />. </param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, writes the string to the file using the
        ///     specified encoding, and then closes the file.
        ///     The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        /// </remarks>
        public void WriteAllText(string content, Encoding encoding)
        {
            FileSystemExtensions.WriteAllText(Path, content, encoding);
        }

        /// <summary>
        ///     Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist,
        ///     this method creates a file, writes the specified string to the file, then closes the file.
        /// </summary>
        /// <param name="content">The content to append.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, appends the string to the end of the file,
        ///     and then closes the file. The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        ///     The method creates the file if it doesn�t exist, but it doesn't create new directories. Therefore, the value of the
        ///     path parameter must contain existing directories.
        /// </remarks>
        public void AppendAllText(string content)
        {
            FileSystemExtensions.AppendAllText(Path, content);
        }

        /// <summary>
        ///     Appends the specified string to the file, creating the file if it does not already exist.
        /// </summary>
        /// <param name="content">The content to append.</param>
        /// <param name="encoding">The encoding to use to encode the text from <paramref name="path" />.</param>
        /// <exception cref="System.ArgumentNullException">content</exception>
        /// <remarks>
        ///     Given a string and a file path, this method opens the specified file, appends the string to the end of the file,
        ///     and then closes the file. The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        ///     The method creates the file if it doesn�t exist, but it doesn't create new directories. Therefore, the value of the
        ///     path parameter must contain existing directories.
        /// </remarks>
        public void AppendAllText(string content, Encoding encoding)
        {
            FileSystemExtensions.AppendAllText(Path, content, encoding);
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
        ///     Opens a file, reads all lines of the file with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding to use to decode the text from <paramref name="path" />.</param>
        /// <remarks>
        ///     This method attempts to automatically detect the encoding of a file based on the presence of byte order marks.
        ///     Encoding formats UTF-8 and UTF-32 (both big-endian and little-endian) can be detected.
        /// </remarks>
        /// <returns>An array of strings containing all lines of the file.</returns>
        public string[] ReadAllLines(Encoding encoding)
        {
            return FileSystemExtensions.ReadAllLines(Path, encoding);
        }

        /// <summary>
        ///     Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <returns>A byte array containing the contents of the file.</returns>
        public byte[] ReadAllBytes()
        {
            return FileSystemExtensions.ReadAllBytes(Path);
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