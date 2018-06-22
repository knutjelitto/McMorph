using System.Collections.Generic;
using System.IO;

namespace McMorph.Recipes
{
    public interface IRecipe : IBase
    {
        string Title { get; }
        List<string> Description { get; }
        List<string> Home { get; }
        string Name { get; }
        string Version { get; }
        string Upstream { get; }
        List<string> Assets { get; }
        List<string> Deps { get; }
        IBuild Build { get; }
    }
}