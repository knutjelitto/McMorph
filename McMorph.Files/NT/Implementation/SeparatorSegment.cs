namespace McMorph.Files.Implementation
{
    public class SeparatorSegment : Segment
    {
        private readonly string separator;

        public static readonly SeparatorSegment Posix = new SeparatorSegment("/");
        public static readonly SeparatorSegment Windows = new SeparatorSegment("\\");

        private SeparatorSegment(string separator)
        {
            this.separator = separator;
        }

        public override string ToString()
        {
            return this.separator;
        }
    }
}