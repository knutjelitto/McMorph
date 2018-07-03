using System;
using System.Collections.Generic;
using System.Linq;

using McMorph.Files.Implementation;

namespace McMorph.Files
{
    public class WindowsPathName : PathName
    {
        internal WindowsPathName(LeadSegment lead, IEnumerable<Segment> segments)
            : base(lead, segments)
        {}
        
        internal override PathName Join(PathName right)
        {
            if (right is WindowsPathName other)
            {
                if (other.IsAbsolute)
                {
                    return other;
                }
                else if (other.IsAnchored)
                {
                    throw new InvalidOperationException("can't join windows-path with anchored windows-path");
                }
                return new WindowsPathName(this.lead, this.segments.Concat(other.segments));
            }
            else
            {
                throw new InvalidOperationException("can't join windows-path with non-windows-path");
            }
        }
    }
}