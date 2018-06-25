namespace McMorph.Processes
{
    public class Output
    {
        private readonly string line;

        public Output(string line)
        {
            this.line = line;
        }

        public override string ToString()
        {
            return this.line;
        }
    }
}