using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class WindowsLeadSegment : LeadSegment
    {
        private static readonly string separator = "\\";
        private readonly string drive;
        private readonly bool root;
        private readonly string toString;

        public WindowsLeadSegment(string drive, bool root)
        {
            this.drive = drive;
            this.root = root;
            this.toString = 
                ((this.drive != string.Empty) ? this.drive + ":" : string.Empty) +
                (this.root ? separator : string.Empty);

        } 

        public override PathName Create(IEnumerable<Segment> segments)
        {
            return new WindowsPathName(this, segments);
        }

        public override string Separator => separator;

        public override bool IsAbsolute => this.drive != string.Empty && this.root;

        public override bool IsAnchored => this.drive != string.Empty || this.root;


        public override string ToString()
        {
            return this.toString;
        }
    }
}