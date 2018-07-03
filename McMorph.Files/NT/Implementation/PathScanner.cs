using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Files.Implementation
{
    internal abstract class PathScanner
    {
        protected string path;
        protected int current;

        public (LeadSegment, List<Segment>) Scan(string path)
        {
            this.path = path ?? string.Empty;
            this.current = 0;

            var lead = First();
            var rest = new List<Segment>(8);
            Segment next;
            while ((next = Next()) != null)
            {
                rest.Add(next);
            }

            return (lead, rest);
        }

        protected abstract bool CurrentIsSep();

        protected abstract LeadSegment First();

        protected bool SkipSeparators()
        {
            var skipped = false;
            while (CurrentIsSep())
            {
                skipped = true;
                Advance();
            }
            return skipped;
        }

        private Segment Next()
        {
            while (Have)
            {
                var start = this.current;

                if (CurrentIs('.'))
                {
                    Advance();
                    if (CurrentIs('.'))
                    {
                        Advance();
                        if (CurrentIsSep())
                        {
                            Advance();
                            while (CurrentIsSep())
                            {
                                Advance();
                            }
                            return new DotDotSegment();
                        }
                        if (!Have)
                        {
                            return new DotDotSegment();
                        }
                    }
                    else
                    {
                        if (SkipSeparators() || !Have)
                        {
                            continue;
                        }
                    }
                }
                
                while (Have && !CurrentIsSep())
                {
                    Advance();
                }

                var segment = new NameSegment(this.path.Substring(start, this.current - start));

                SkipSeparators();

                return segment;
            }

            return null;
        }

        protected bool Have => this.current < this.path.Length;

        protected bool DontHave => this.current >= this.path.Length;

        protected bool Advance()
        {
            if (this.current < this.path.Length)
            {
                this.current += 1;
            }
            return this.current < this.path.Length;
        }

        protected bool CurrentIs(char ch)
        {
            return this.current < this.path.Length && this.path[this.current] == ch;
        }

        protected bool CurrentIs(Func<char, bool> predicate)
        {
            return this.current < this.path.Length && predicate(this.path[this.current]);
        }

        protected bool NextIs(char ch)
        {
            return this.current + 1 < this.path.Length && this.path[this.current + 1] == ch;
        }

        protected bool NextIs(Func<char, bool> predicate)
        {
            return this.current + 1 < this.path.Length && predicate(this.path[this.current + 1]);
        }

        protected bool AfterNextIs(char ch)
        {
            return this.current + 2 < this.path.Length && this.path[this.current + 2] == ch;
        }

        protected bool AfterNextIs(Func<char, bool> predicate)
        {
            return this.current + 2 < this.path.Length && predicate(this.path[this.current + 2]);
        }
    }
}