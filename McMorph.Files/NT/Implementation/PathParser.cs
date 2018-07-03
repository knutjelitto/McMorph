using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Files.Implementation
{
    internal class PathParser
    {
        private readonly PathScanner scanner;
        private readonly Func<LeadSegment, IEnumerable<Segment>, PathName> create;

        public PathParser(PathScanner scanner, Func<LeadSegment, IEnumerable<Segment>, PathName> create)
        {
            this.scanner = scanner;
            this.create = create;
        }

        public PathName Parse(string path)
        {
            var (lead, rest) = this.scanner.Scan(path);
            return this.create(lead, Reduce(rest));
        }

        private IEnumerable<Segment> Reduce(List<Segment> segments)
        {
            foreach (var segment in segments)
            {
                if (segment is NameSegment) // name or '..'
                {
                    yield return segment;
                }
            }
        }
    }
}   