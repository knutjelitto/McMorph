using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Files.Implementation
{
    internal class PathScanner
    {
        protected string path;
        protected int current;
        protected List<Segment> segments = new List<Segment>(10);

        protected void Setup(string path)
        {
            this.path = path;
            this.current = 0;
            this.segments.Clear();
        }

        protected bool Have => this.current < this.path.Length;

        protected bool NoNext => this.current + 1 >= this.path.Length;

        protected void Add(Segment segment)
        {
            this.segments.Add(segment);
        }

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