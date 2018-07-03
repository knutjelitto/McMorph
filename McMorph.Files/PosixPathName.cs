using System;
using System.Collections.Generic;
using System.Linq;

using McMorph.Files.Implementation;

namespace McMorph.Files
{
    public class PosixPathName : PathName
    {
        internal PosixPathName(LeadSegment lead, IEnumerable<Segment> segments)
            : base(lead, segments)
        {}

        internal override PathName Join(PathName right)
        {
            if (right is PosixPathName other)
            {
                if (other.IsAbsolute)
                {
                    return right;
                }
                return new PosixPathName(this.lead, this.segments.Concat(other.segments));
            }
            else
            {
                throw new InvalidOperationException("can't join posix-path with non-posix-path");
            }
        }
    }
}