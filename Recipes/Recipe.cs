using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace McMorph.Recipes
{
    public class Recipe : Base, IRecipe
    {
        public string Title { get; set; }

        public List<string> Description { get; } = new List<string>();

        public List<string> Home { get; } = new List<string>();

        public string Name { get; set; }

        public string Version { get; set; }

        public string Upstream { get; set; }

        public List<string> Assets { get; } = new List<string>();

        public List<string> Deps { get; } = new List<string>();

        public RecipeClass Class { get; set; } = RecipeClass.None;
        
        public IBuild Build { get; set; }

        public override void Dump(TextWriter writer)
        {
            Single(writer, "Title", Title);
            Multi(writer, "Description", Description);
            Multi(writer, "Home", Home);
            Single(writer, "Name", Name);
            Single(writer, "Version", Version);
            Single(writer, "Upstream", Upstream);
            Multi(writer, "Assets", Assets);
            Multi(writer, "Deps", Deps);
            if (Build != null)
            {
                Build.Dump(writer);
            }
        }
    }
}