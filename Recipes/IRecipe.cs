using System.Collections.Generic;
using System.IO;

namespace McMorph.Recipes
{
    public interface IRecipe : IBase
    {
        string Title { get; set; }
        List<string> Description { get; set; }
        List<string> Home { get; }
        string Name { get; set; }
        string Version { get; set; }
        List<string> Upstream { get; }
        List<string> Assets { get; }
        List<string> Deps { get; }
        IBuild Build { get; set; }
    }
}