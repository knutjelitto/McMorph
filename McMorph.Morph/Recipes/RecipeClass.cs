using System;

namespace McMorph.Recipes
{
    [Flags]
    public enum RecipeClass
    {
        None = 0,
        Optional = 1,
        Core = 2,
        Gnu = 4,
        Linux = 8,
        Other = 16,
    }
}