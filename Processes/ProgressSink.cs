using System;
using System.Threading.Tasks;

namespace McMorph.Processes
{
    public class ProgressSink : ISink
    {
        private readonly ISink sink;
        private int state;
        private string[] gimmik = 
        {
            "[>---<]\b\b\b\b\b\b\b",
            "[=>---]\b\b\b\b\b\b\b",
            "[<=>--]\b\b\b\b\b\b\b",
            "[-<=>-]\b\b\b\b\b\b\b",
            "[--<=>]\b\b\b\b\b\b\b",
            "[---<=]\b\b\b\b\b\b\b",
        };

        public ProgressSink(ISink sink)
        {
            this.sink = sink;
            this.state = 0;
        }

        public ProgressSink() : this(new NullSink())
        {
        }

        public void PutLine(string line)
        {
            this.sink.PutLine(line);
            Console.Write(this.gimmik[this.state]);
            this.state = (this.state + 1) % this.gimmik.Length;
            Task.Delay(100).Wait();
        }
    }
}