using System.Collections.Generic;

using McMorph.Files.Implementation;

namespace McMorph.Files
{
    public class WindowsPathName : PathName
    {
        internal WindowsPathName(LeadSegment lead, IEnumerable<Segment> segments)
            : base(lead, segments)
        {}
    }
}