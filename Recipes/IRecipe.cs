using System;
using System.Collections.Generic;

namespace McMorph.Recipes
{
    public interface IRecipe : IBase
    {
        string Title { get; }
        List<string> Description { get; }
        List<string> Home { get; }
        string Name { get; }
        string Version { get; }
        Uri Upstream { get; }
        List<string> Assets { get; }
        List<string> Deps { get; }
        IBuild Build { get; }
    }
}