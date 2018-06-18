namespace McMorph.Recipes
{
    public class AbsTag : Tag
    {
        public AbsTag(int lineno)
            : base (lineno)
        {}

        public override string ToString()
        {
            return string.Join('.', Parts);
        }
    }
}