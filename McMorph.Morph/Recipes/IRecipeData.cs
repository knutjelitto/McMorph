using System;
using System.Collections.Generic;

namespace McMorph.Recipes
{
    public interface IRecipeData : IBase
    {
        string Title { get; }
        List<string> Description { get; }
        List<string> Home { get; }
        string Name { get; }
        string Version { get; }
        string Upstream { get; }
        List<string> Assets { get; }
        List<string> Deps { get; }
        IBuildData Build { get; }
    }
}