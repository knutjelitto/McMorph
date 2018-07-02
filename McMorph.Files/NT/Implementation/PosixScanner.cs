using System;
using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class PosixScanner : PathScanner
    {
        public IEnumerable<Segment> Parse(string path)
        {
            Setup(path);

            while (Have)
            {
                yield return Next();
            }
        }

        private Segment Next()
        {
            if (CurrentIs('/'))
            {
                Advance();
                return new SeparatorSegment('/');
            }
            
            var start = this.current;
            
            if (CurrentIs('.'))
            {

                Advance();
                if (CurrentIs('.') && (NextIs('/') || NoNext))
                {
                    Advance();
                    return new DotDotSegment();
                }
                if (NextIs('/') || NoNext)
                {
                    return new DotSegment();
                }
            }
            
            while (Have && !CurrentIs('/'))
            {
                Advance();
            }
            return new NameSegment(this.path.Substring(start, this.current - start));
        }
    }
}