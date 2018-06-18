using System.Collections.Generic;

namespace McMorph.Recipes
{
    public class Tag : Token
    {
        public Tag(int lineNo)
            : base(lineNo)
        {}
        
        public List<string> Parts { get; } = new List<string>();
        public bool IsMulti => Parts.Count > 1;
        public bool IsSingle => Parts.Count == 1;
    }
}