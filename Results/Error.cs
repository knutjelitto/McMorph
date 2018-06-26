using System;
using System.IO;

using McMorph.Files;

namespace McMorph.Results
{

    [System.Serializable]
    public class ErrorException : ApplicationException
    {
        public ErrorException() { }
        public ErrorException(string message) : base(message) { }
        public ErrorException(string message, System.Exception inner) : base(message, inner) { }
        protected ErrorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public static class Assert
    {
        public static void ThrowIfArgumentNotNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }

    public static class Error
    {
        public static FileNotFoundException NewFileNotFoundException(UPath path)
        {
            return new FileNotFoundException($"expected file '{path}', but doesn't exists");
        }

        public static DirectoryNotFoundException NewDirectoryNotFoundException(UPath path)
        {
            return new DirectoryNotFoundException($"expected directory '{path}', but doesn't exists");
        }

        public static Exception ExistsButIsNotFile(UPath path)
        {
            return new FileNotFoundException($"'{path}' exists, but isn't a file as expected");
        }

        public static Exception NewEntryDoesntExists(UPath path)
        {
            return new FileNotFoundException($"'{path}' doesn't exists as expected");
        }

        public static void Throw(string message)
        {
            throw new ErrorException(message);
        }
    }
}