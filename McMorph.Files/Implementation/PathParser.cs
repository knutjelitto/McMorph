using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Files.Implementation
{
    internal class PathParser
    {
        private readonly Func<LeadSegment, IEnumerable<Segment>, PathName> createPathName;
        private readonly Func<string, PathScanner> createScanner;

        public PathParser(Func<string, PathScanner> createScanner, Func<LeadSegment, IEnumerable<Segment>, PathName> createPathName)
        {
            this.createScanner = createScanner;
            this.createPathName = createPathName;
        }

        public PathName Parse(string path)
        {
            var (lead, rest) = this.createScanner(path).Scan();
            return this.createPathName(lead, Reduce(rest));
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