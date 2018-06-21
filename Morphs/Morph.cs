using McMorph.Recipes;

namespace McMorph.Morphs
{
    public class Morph
    {
        public readonly Recipe Recipe;

        public Morph(Recipe recipe)
        {
            this.Recipe = recipe;
        }

        public string Name => this.Recipe.Name;

        public string Version => this.Recipe.Version;

        public string Tag => Name + "-" + Version;

    }
}