using System.Collections.Generic;
using McMorph.Files.Implementation;

namespace McMorph.Files.NT
{
    public class PosixPath : PurePosixPath, IPath
    {
        internal PosixPath(IEnumerable<Segment> segments) : base(segments)
        {
        }
    }
}