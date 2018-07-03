// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using IOPath=System.IO.Path;

namespace McMorph.Files
{
    /// <summary>
    /// A uniform unix like path.
    /// </summary>
    /// <seealso cref="UPath" />
    public struct UPath : IEquatable<UPath>, IComparable<UPath>
    {
        /// <summary>
        /// An empty path.
        /// </summary>
        public static readonly UPath Empty = new UPath(string.Empty, true);

        /// <summary>
        /// The root path `/`
        /// </summary>
        public static readonly UPath Root = new UPath("/", true);

        /// <summary>
        /// The default comparer for a <see cref="UPath"/> that is case sensitive.
        /// </summary>
        public static readonly IComparer<UPath> DefaultComparer = new ComparerCaseSensitive();

        /// <summary>
        /// The default comparer for a <see cref="UPath"/> that is case insensitive.
        /// </summary>
        public static readonly IComparer<UPath> DefaultComparerIgnoreCase = new ComparerIgnoreCase();

        /// <summary>
        /// Initializes a new instance of the <see cref="UPath"/> struct.
        /// </summary>
        /// <param name="path">The path that will be normalized.</param>
        public UPath(string path) : this(path, false)
        {
        }

        internal UPath(string path, bool safe)
        {
            if (safe)
            {
                FullName = path;
            }
            else
            {
                FullName = ValidateAndNormalize(path);
            }
        }

        /// <summary>
        /// Gets the full name of this path (Note that it may be null).
        /// </summary>
        /// <value>The full name of this path.</value>
        public string FullName { get; }

        /// <summary>
        /// Gets a value indicating whether this path is null.
        /// </summary>
        /// <value><c>true</c> if this instance is null; otherwise, <c>false</c>.</value>
        public bool IsNull => FullName == null;

        /// <summary>
        /// Gets a value indicating whether this path is empty (<see cref="FullName"/> equals to the empty string)
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty => FullName == string.Empty;

        /// <summary>
        /// Gets a value indicating whether this path is absolute by starting with a leading `/`.
        /// </summary>
        /// <value><c>true</c> if this path is absolute; otherwise, <c>false</c>.</value>
        public bool IsAbsolute => FullName?.StartsWith("/") ?? false;

        /// <summary>
        /// Gets a value indicating whether this path is relative by **not** starting with a leading `/`.
        /// </summary>
        /// <value><c>true</c> if this instance is relative; otherwise, <c>false</c>.</value>
        public bool IsRelative => !IsAbsolute;

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="UPath"/>.
        /// </summary>
        /// <param name="path">The path as a string.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator UPath(string path)
        {
            if (path == null)
            {
                return Empty;
            }
            return new UPath(path);
        }

        /// <summary>
        /// Performs an explict conversion from <see cref="UPath"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The result as a string of the conversion.</returns>
        public static explicit operator string(UPath path)
        {
            return path.FullName;
        }

        /// <summary>
        /// Combines two paths into a new path.
        /// </summary>
        /// <param name="path1">The first path to combine.</param>
        /// <param name="path2">The second path to combine.</param>
        /// <returns>The combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If path2 contains an absolute path, this method returns path2.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// path1
        /// or
        /// path2
        /// </exception>
        /// <exception cref="System.ArgumentException">If an error occurs while trying to combine paths.</exception>
        public static UPath Combine(UPath path1, UPath path2)
        {
            return IOPath.Combine(path1.FullName, path2.FullName);
        }

        public static UPath Combine(UPath path1, UPath path2, UPath path3)
        {
            return IOPath.Combine(path1.FullName, path2.FullName, path3.FullName);
        }

        public static UPath Combine(UPath path1, UPath path2, UPath path3, UPath path4)
        {
            return IOPath.Combine(path1.FullName, path2.FullName, path3.FullName, path4.FullName);
        }

        public static UPath Combine(params UPath[] paths)
        {
            var path = paths[0];

            for (var i = 1; i < paths.Length; i++)
                path = Combine(path, paths[i]);

            return path;
        }

        /// <summary>
        /// Implements the / operator equivalent of <see cref="Combine"/>
        /// </summary>
        /// <param name="path1">The first path to combine.</param>
        /// <param name="path2">The second path to combine.</param>
        /// <returns>The combined paths. If one of the specified paths is a zero-length string, this method returns the other path. If path2 contains an absolute path, this method returns path2.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// path1
        /// or
        /// path2
        /// </exception>
        /// <exception cref="System.ArgumentException">If an error occurs while trying to combine paths.</exception>
        public static UPath operator /(UPath path1, UPath path2)
        {
            return Combine(path1, path2);
        }

        /// <inheritdoc />
        public bool Equals(UPath other)
        {
            return string.Equals(this.FullName, other.FullName);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is UPath upath && Equals(upath);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return FullName?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(UPath left, UPath right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(UPath left, UPath right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="UPath"/>
        /// </summary>
        /// <param name="path">The path as a string.</param>
        /// <param name="pathInfo">The path parsed if successfull.</param>
        /// <returns><c>true</c> if path was parsed successfully, <c>false</c> otherwise.</returns>
        public static bool TryParse(string path, out UPath pathInfo)
        {
            try
            {
                pathInfo = ValidateAndNormalize(path);
                return true;
            }
            catch
            {
                pathInfo = null;
                return false;
            }
        }

        private static string ValidateAndNormalize(string path)
        {
            if (path == null)
            {
                return string.Empty;
            }
            if (IOPath.IsPathRooted(path))
            {
                return IOPath.GetFullPath(path);
            }
            if (path == ".." || path == ".")
            {
                return path;
            }
            return IOPath.GetFullPath(path).Substring(Environment.CurrentDirectory.Length + 1);
        }

        /// <inheritdoc />
        public int CompareTo(UPath other)
        {
            return string.Compare(this.FullName, other.FullName, StringComparison.Ordinal);
        }

        private class ComparerCaseSensitive : IComparer<UPath>
        {
            public int Compare(UPath x, UPath y)
            {
                return string.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
            }
        }

        private class ComparerIgnoreCase : IComparer<UPath>
        {
            public int Compare(UPath x, UPath y)
            {
                return string.Compare(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool Exists => FileSystem.Instance.FileExists(this) || FileSystem.Instance.DirectoryExists(this);

        public FileEntry AsFile => new FileEntry(this);
        public DirectoryEntry AsDirectory => new DirectoryEntry(this);
    }
}