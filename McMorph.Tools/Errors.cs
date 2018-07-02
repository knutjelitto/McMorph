using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace McMorph.Tools
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
        public static void ThrowIfArgumentNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }

    public static class Errors
    {
        public static Exception NewProcessError(string command, List<string> errors, int exitCode)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"error executing '{command}'");
            builder.AppendLine($"exit code is: {exitCode}");
            builder.AppendLine($"message is:");
            foreach (var error in errors)
            {
                builder.AppendLine(error);
            }

            return new ErrorException(builder.ToString());
        }

        public static void Throw(string message)
        {
            throw new ErrorException(message);
        }
    }
}