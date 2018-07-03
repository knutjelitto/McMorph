using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class WindowsLeadSegment : LeadSegment
    {
        private static readonly string separator = "\\";
        private readonly string drive;
        private readonly bool root;

        public WindowsLeadSegment(string drive, bool root)
        {
            this.drive = drive;
            this.root = root;
        }

        public override PathName Create(IEnumerable<Segment> segments)
        {
            return new WindowsPathName(this, segments);
        }

        public override string Separator => separator;

        public override string ToString()
        {
            return this.drive + (this.root ? separator : string.Empty);
        }
    }
}