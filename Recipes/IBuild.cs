namespace McMorph.Recipes
{
    public interface IBuild
    {
         void Configure();
         void Make();
         void Install();

    }
}