using System.Collections.Generic;

using McMorph.Files.Implementation;

namespace McMorph.Files
{
    public class PosixPathName : PathName
    {
        internal PosixPathName(LeadSegment lead, IEnumerable<Segment> segments)
            : base(lead, segments)
        {}
    }
}