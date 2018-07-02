using System.Collections.Generic;

namespace McMorph.Recipes
{
    public interface IBuildData : IBase
    {
        List<string> Settings { get; }
        List<string> Configure { get; }
        List<string> Make { get; }
        List<string> Install { get; }
    }
}