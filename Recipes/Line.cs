using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Recipes
{
    public class Line
    {
        private readonly Lines lines;
        public readonly int index;
        private int offset;
        private readonly string text;

        internal Line(Lines lines, int index)
        {
            this.lines = lines;
            this.index = index;
            this.offset = 0;
            this.text = this.lines.Text(this.index);
            Parse();
        }

        public Line Next
        {
            get => this.lines.Next(this);
        }

        public bool IsRelative { get; private set; }

        public List<string> Tags { get; private set; }

        public string Value { get; private set; }

        public bool IsTag => Tags != null;
        public bool IsValue => Value != null;
        public bool IsTagValue => IsTag && IsValue;
        public bool IsComment => Tags == null && Value != null && Value.StartsWith('#');

        private bool Has => this.offset < this.text.Length;
        private char Char => this.offset < this.text.Length ? this.text[this.offset] : '\uFFFF';

        private void SkipWhitespace()
        {
            while (Has && char.IsWhiteSpace(Char))
            {
                this.offset++;
            }
        }

        private void SkipComment()
        {
            SkipWhitespace();
            if (Has && Char == '#')
            {
                this.offset = this.text.Length;
            }
        }

        private string ParseTag()
        {
            SkipWhitespace();
            int start = this.offset;
            while (Has && char.IsLetterOrDigit(Char))
            {
                this.offset++;
            }
            var length = this.offset - start;
            if (length > 0)
            {
                return text.Substring(start, length);
            }
            return null;
        }

        private void Parse()
        {
            int dotcount = 0;
            SkipComment();
            if (!Has)
            {
                Value = string.Empty;
                return;
            }
            if (Char == '[')
            {
                offset++;
                if (Has && Char == '.')
                {
                    dotcount++;
                    offset++;
                    this.IsRelative = true;
                }
                this.Tags = new List<string>();
                while (Has)
                {
                    var tag = ParseTag();
                    if (tag == null)
                    {
                        break;
                    }
                    this.Tags.Add(tag);
                    if (Has && Char == '.')
                    {
                        dotcount++;
                        offset++;
                        continue;
                    }
                    break;
                }
                if (!Has ||
                    Char != ']' ||
                    (this.IsRelative && this.Tags.Count != dotcount) ||
                    (!this.IsRelative && this.Tags.Count != dotcount + 1))
                {
                    Debug.Assert(false);
                    return;
                }
                offset++;
                SkipWhitespace();
                if (Has)
                {
                    var start = offset;
                    offset = text.Length;
                    while (--offset > start && char.IsWhiteSpace(Char))
                    {
                    }
                    var length = offset - start + 1;
                    Value = text.Substring(start, length);
                }
            }
            else
            {
                Value = text;
            }
        }
    }
}