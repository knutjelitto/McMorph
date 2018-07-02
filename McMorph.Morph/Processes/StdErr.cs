namespace McMorph.Processes
{
    public class StdErr : Output
    {
        public StdErr(string line) : base(line)
        {
        }

        public override string ToString()
        {
            return "E: " + base.ToString();
        }
    }
}