using System.Collections.Generic;

namespace McMorph.Recipes
{
    public class Lines
    {
        private readonly string[] texts;
        private readonly List<Line> lines = new List<Line>();

        public Lines(string[] texts)
        {
            this.texts = texts;
        }

        public int Count => this.texts.Length;

        public string Text(int index) => this.texts[index];

        public Line First()
        {
            return Ensure(0);
        }

        public Line Next(Line line)
        {
            return Ensure(line.index + 1);
        }

        private Line Ensure(int index)
        {
            if (index < this.texts.Length)
            {
                while (lines.Count <= index)
                {
                    lines.Add(new Line(this, lines.Count));
                }
                return this.lines[index];
            }
            return null;
        }
    }
}