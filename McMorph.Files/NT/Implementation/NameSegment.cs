namespace McMorph.Files.Implementation
{
    public class NameSegment : Segment
    {
        private readonly string name;
        
        public NameSegment (string name)
        {
            this.name = name;

        }

        public string Name => this.name;

        public override string ToString()
        {
            return this.name;
        }
    }
}