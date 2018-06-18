using System;

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

    public static class Error
    {
        public static void Throw(string message)
        {
            throw new ErrorException(message);
        }
    }
}