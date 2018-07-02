namespace McMorph.Recipes
{
    public class RelTag : Tag
    {
        public RelTag(int lineNo)
            : base (lineNo)
        {}
 
        public override string ToString()
        {
            return "." + string.Join(',', Parts);
        }
   }
}