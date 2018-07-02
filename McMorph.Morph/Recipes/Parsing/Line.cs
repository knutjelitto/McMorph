using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace McMorph.Recipes
{
    public class Line : IEnumerable<Token>
    {
        private readonly int lineNo;
        private readonly string text;
        private int offset;

        internal Line(int no, string text)
        {
            this.lineNo = no;
            this.text = text;
            this.offset = 0;
        }

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

        private string ParsePart()
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

        public IEnumerator<Token> GetEnumerator()
        {
            SkipComment();
            if (!Has)
            {
                yield break;
            }
            if (Char == '[')
            {
                offset++;
                SkipWhitespace();
                Tag tag;
                if (Has && Char == '.')
                {
                    tag = new RelTag(this.lineNo);
                    offset++;
                }
                else
                {
                    tag = new AbsTag(this.lineNo);
                }
                var dot = false;
                while (Has)
                {
                    var part = ParsePart();
                    if (part == null)
                    {
                        if (dot)
                        {
                            yield return new ErrTok(this.lineNo, $"superfluous dot in tag");
                            yield break;
                        }
                        break;
                    }
                    dot = false;
                    tag.Parts.Add(part);
                    SkipWhitespace();
                    if (Has && Char == '.')
                    {
                        offset++;
                        SkipWhitespace();
                        dot = true;
                        continue;
                    }
                    break;
                }
                SkipWhitespace();
                if (!Has || Char != ']')
                {
                    yield return new ErrTok(this.lineNo, $"missing tag closer");
                }
                offset++;
                if (tag.Parts.Count == 0)
                {
                    yield return new ErrTok(this.lineNo, $"empty tag");
                    yield break;
                }
                yield return tag;

                SkipWhitespace();
                if (Has)
                {
                    var start = offset;
                    offset = text.Length;
                    while (--offset > start && char.IsWhiteSpace(Char))
                    {
                    }
                    var length = offset - start + 1;
                    if (length > 0)
                    {
                        yield return new Value(this.lineNo, text.Substring(start, length));
                    }
                }
            }
            else
            {
                yield return new Value(this.lineNo, text);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}