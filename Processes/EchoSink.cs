using System;

namespace McMorph.Processes
{
    public class EchoSink : ISink
    {
        private readonly ISink echo;

        public EchoSink(ISink echo)
        {
            this.echo = echo;
        }

        public void PutLine(string line)
        {
            Console.WriteLine(line);
            echo.PutLine(line);
        }
    }
}