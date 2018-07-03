using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class PosixLeadSegment : LeadSegment
    {
        private static readonly string separator = "/";
        private readonly bool root;

        public PosixLeadSegment(bool root)
        {
            this.root = root;
        }

        public override PathName Create(IEnumerable<Segment> segments)
        {
            return new PosixPathName(this, segments);
        }

        public override string Separator => separator;

        public override bool IsAbsolute => this.root;

        public override bool IsAnchored => this.root;

        public override string ToString()
        {
            return this.root ? Separator : string.Empty;
        }
    }
}