namespace McMorph.Processes
{
    public class Output
    {
        public readonly string Line;

        public Output(string line)
        {
            this.Line = line;
        }

        public override string ToString()
        {
            return this.Line;
        }
    }
}