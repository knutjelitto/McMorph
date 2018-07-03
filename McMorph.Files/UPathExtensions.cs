// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace McMorph.Files
{
    /// <summary>
    /// Extension methods for <see cref="UPath"/>
    /// </summary>
    public static class UPathExtensions
    {

        /// <summary>
        /// Gets the file or last directory name and extension of the specified path.
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension.</param>
        /// <returns>The characters after the last directory character in path. If path is null, this method returns null.</returns>
        public static string GetName(this UPath path)
        {
            return path.IsNull ? null : Path.GetFileName(path.FullName);
        }

        /// <summary>
        /// Gets the file or last directory name without the extension for the specified path.
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name without the extension.</param>
        /// <returns>The characters after the last directory character in path without the extension. If path is null, this method returns null.</returns>
        public static string GetNameWithoutExtension(this UPath path)
        {
            return path.IsNull ? null : Path.GetFileNameWithoutExtension(path.FullName);
        }

        /// <summary>
        /// Gets the extension of the specified path.
        /// </summary>
        /// <param name="path">The path string from which to obtain the extension with a leading dot `.`.</param>
        /// <returns>The extension of the specified path (including the period "."), or null, or String.Empty. If path is null, GetExtension returns null. If path does not have extension information, GetExtension returns String.Empty..</returns>
        public static string GetExtensionWithDot(this UPath path)
        {
            return path.IsNull ? null : Path.GetExtension(path.FullName);
        }

        /// <summary>
        /// Asserts the specified path is not null.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name of a parameter to include n the <see cref="ArgumentNullException"/>.</param>
        /// <returns>A path not modified.</returns>
        /// <exception cref="System.ArgumentNullException">If the path was null using the parameter name from <paramref name="name"/></exception>
        public static UPath AssertNotNull(this UPath path, string name = "path")
        {
            if (path.FullName == null)
                throw new ArgumentNullException(name);
            return path;
        }

        /// <summary>
        /// Asserts the specified path is absolute.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="argumentName">The name of a parameter to include n the <see cref="ArgumentNullException"/>.</param>
        /// <returns>A path not modified.</returns>
        /// <exception cref="System.ArgumentException">If the path is not absolute using the parameter name from <paramref name="argumentName"/></exception>
        public static UPath AssertAbsolute(this UPath path, string argumentName = "path")
        {
            AssertNotNull(path, argumentName);

            if (!path.IsAbsolute)
                throw new ArgumentException($"Path `{path}` must be absolute", argumentName);
            return path.FullName;
        }
    }
}