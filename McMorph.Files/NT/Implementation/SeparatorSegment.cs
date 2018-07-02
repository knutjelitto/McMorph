namespace McMorph.Files.Implementation
{
    public class SeparatorSegment : Segment
    {
        private readonly char separator;

        public SeparatorSegment(char separator)
        {
            this.separator = separator;
        }


        public override string ToString()
        {
            return this.separator.ToString();
        }
    }
}