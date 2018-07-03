using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace McMorph.Files
{
    using McMorph.Files.Implementation;

    public class PurePosixPath : PurePath
    {
        [ThreadStatic] static PosixScanner scanner = new PosixScanner();
        [ThreadStatic] static PosixParser parser = new PosixParser();
        private readonly IReadOnlyList<Segment> segments;
        private readonly bool isRooted;

        internal PurePosixPath(IEnumerable<Segment> segments)
            : base(PathFlawor.Posix)
        {
            this.segments = segments.ToList();
            this.isRooted = this.segments.Count > 0 && this.segments[0] is SeparatorSegment;
            if (this.isRooted)
            {
                this.segments = this.segments.Skip(1).ToList();
            }
        }

        public static PurePosixPath Parse(string path)
        {
            return new PurePosixPath(parser.Reduce(scanner.Parse(path)));
        }

        public override string ToString()
        {
            return (this.isRooted ? "/" : string.Empty) + string.Join('/', this.segments);
        }
    }
}