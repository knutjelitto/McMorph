using System;
using System.Collections.Generic;
using System.IO;

namespace McMorph.Recipes
{
    public abstract class Base
    {
        protected void Multi(TextWriter writer, string tag, List<string> values)
        {
            writer.WriteLine($"[{tag}]");
            if (values.Count > 0)
            {
                writer.WriteLine($"{string.Join(Environment.NewLine, values)}");
            }
        }

        protected void Single(TextWriter writer, string tag, string value)
        {
            writer.WriteLine($"[{tag}] {value}");
        }

        public abstract void Dump(TextWriter writer);
    }
}