namespace McMorph.Processes
{
    public class StdOut : Output
    {
        public StdOut(string line) : base(line)
        {
        }

        public override string ToString()
        {
            return "O: " + base.ToString();
        }
    }
}