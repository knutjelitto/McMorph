using System;
using System.Collections.Generic;

namespace McMorph.Recipes
{
    public class Value : Token
    {
        public Value(int lineNo, string text)
            : base(lineNo)
        {
            Text = text;
        }

        public string Text { get; }
    }
}