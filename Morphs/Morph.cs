using System;
using System.Collections.Generic;

using McMorph.Recipes;

namespace McMorph.Morphs
{
    public class Morph
    {
        public Morph(Pogo pogo, Recipe recipe)
        {
            this.Pogo = pogo;
            this.Recipe = recipe;
        }

        public Pogo Pogo { get; }
        public Recipe Recipe { get; }

        public string Name => this.Recipe.Name;

        public string Version => this.Recipe.Version;

        public string Tag => this.Name + "-" + this.Version;

        public Upstream Upstream => new Upstream(this, this.Recipe.Upstream);

        public IEnumerable<Asset> Assets
        {
            get
            {
                foreach (var assetUri in this.Recipe.Assets)
                {
                    yield return new Asset(this, assetUri);
                }
            }
        }
    }
}
