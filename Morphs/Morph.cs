using System.Collections.Generic;

using McMorph.Recipes;

namespace McMorph.Morphs
{
    public class Morph
    {
        private readonly Pogo pogo;
        private readonly Recipe Recipe;

        public Morph(Pogo pogo, Recipe recipe)
        {
            this.pogo = pogo;
            this.Recipe = recipe;
        }

        public string Name => this.Recipe.Name;

        public string Version => this.Recipe.Version;

        public string Tag => Name + "-" + Version;

        public Upstream Upstream => new Upstream(this.pogo, this);

        public string UpstreamValue => Recipe.Upstream;
    }
}