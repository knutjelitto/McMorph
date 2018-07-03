using System.Collections.Generic ;

namespace McMorph.Files.Implementation
{
    internal abstract class LeadSegment : Segment
    {
        public abstract string Separator { get; }

        public abstract bool IsAbsolute { get; }

        public bool IsRelative => !IsAbsolute && !IsAnchored;

        public abstract bool IsAnchored { get; }

        public abstract PathName Create(IEnumerable<Segment> segments);
    }
}