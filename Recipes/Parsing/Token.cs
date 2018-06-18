namespace McMorph.Recipes
{
    public class Token
    {
        public Token(int lineNo)
        {
            LineNo = lineNo;
        }

        public int LineNo { get; }
    }
}