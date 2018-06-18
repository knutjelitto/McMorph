using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace McMorph.Recipes
{
    public class Recipe : Base
    {
        public string Title { get; internal set; }

        public List<string> Description { get; } = new List<string>();

        public List<string> Home { get; } = new List<string>();

        public string Name { get; internal set; }

        public string Version { get; internal set; }

        public List<string> Upstream { get; } = new List<string>();

        public List<string> Assets { get; } = new List<string>();

        public List<string> Deps { get; } = new List<string>();
        
        public IBuild Build { get; internal set; }

        public override void Dump(TextWriter writer)
        {
            Single(writer, "Title", Title);
            Multi(writer, "Description", Description);
            Multi(writer, "Home", Home);
            Single(writer, "Name", Name);
            Single(writer, "Version", Version);
            Multi(writer, "Upstream", Upstream);
            Multi(writer, "Assets", Assets);
            Multi(writer, "Deps", Deps);
            if (Build != null)
            {
                Build.Dump(writer);
            }
        }
    }
}