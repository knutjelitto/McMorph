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
        private readonly IEnumerable<Segment> segments;

        internal PurePosixPath(IEnumerable<Segment> segments)
        {
            this.segments = segments;
        }

        public static PurePosixPath Parse(string path)
        {
            return new PurePosixPath(parser.Reduce(scanner.Parse(path)));
        }

        public override string ToString()
        {
#if false
            return string.Join('/', this.segments);
#else
            var xxx = new StringBuilder();
            foreach (var segment in this.segments)
            {
                if (segment is DotSegment)
                {
                    xxx.Append("<.>");
                }
                else if (segment is NameSegment nameSegment)
                {
                    xxx.Append($"<name:{nameSegment.Name}");
                }
                else
                {
                    xxx.Append("<?>");
                }
            }
            return xxx.ToString();
#endif
        }
    }
}