namespace McMorph.Processes
{
    public class TeeSink : ISink
    {
        private readonly ISink[] sinks;

        public TeeSink(params ISink[] sinks)
        {
            this.sinks = sinks;
        }

        public void PutLine(string line)
        {
            foreach (var sink in this.sinks)
            {
                sink.PutLine(line);
            }
        }
    }
}