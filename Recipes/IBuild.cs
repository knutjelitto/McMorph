using System.Collections.Generic;

namespace McMorph.Recipes
{
    public interface IBuild : IBase
    {
         List<string> Configure { get; }
         List<string> Make { get; }
         List<string> Install { get; }
    }
}