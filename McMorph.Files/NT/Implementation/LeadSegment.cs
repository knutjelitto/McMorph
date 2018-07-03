using System.Collections.Generic ;

namespace McMorph.Files.Implementation
{
    internal abstract class LeadSegment : Segment
    {
        public abstract string Separator { get; }

        public abstract PathName Create(IEnumerable<Segment> segments);
    }
}