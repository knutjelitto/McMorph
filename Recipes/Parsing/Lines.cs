using System.Collections;
using System.Collections.Generic;

namespace McMorph.Recipes
{
    public class Lines : IEnumerable<Token>
    {
        private readonly string[] texts;
        private readonly List<Line> lines = new List<Line>();

        public Lines(string[] texts)
        {
            this.texts = texts;
        }

        public int Count => this.texts.Length;

        public IEnumerator<Token> GetEnumerator()
        {
            int lineNo = 0;
            foreach (var text in this.texts)
            {
                var line = new Line(++lineNo, text);
                foreach (var tok in line)
                {
                    yield return tok;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}