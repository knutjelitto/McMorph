using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Files.Implementation
{
    public class PathParser
    {
        private enum State
        {
            Intro,
            Content,
        }

        public IEnumerable<Segment> Reduce(IEnumerable<Segment> segments)
        {
            var state = State.Intro;
            var yielded = false;

            foreach (var segment in segments)
            {
                switch (state)
                {
                    case State.Intro:
                        if (segment is SeparatorSegment)
                        {
                            if (!yielded)
                            {
                                yield return new RootSegment();
                                yielded = true;
                            }
                            break;
                        }
                        else
                        {
                            state = State.Content;
                            goto case State.Content;
                        }
                    case State.Content:
                        if (segment is DotDotSegment || segment is NameSegment)
                        {
                            yield return segment;
                            yielded = true;
                        }
                        else if (segment is DotSegment || segment is SeparatorSegment)
                        {
                            // nop
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                        break;
                }
            }

            if (!yielded)
            {
                yield return new DotSegment();
            }
        }
    }
}