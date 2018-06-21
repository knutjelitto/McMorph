using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace McMorph.Morphs
{
    public class MorphCollection : IReadOnlyList<Morph>, IReadOnlyDictionary<string, Morph>
    {
        private List<Morph> list = new List<Morph>();
        private Dictionary<string, Morph> lookup = new Dictionary<string, Morph>();

        public void Add(Morph morph)
        {
            this.lookup.Add(morph.Name, morph);
            this.list.Add(morph);
        }

        public static MorphCollection Populate()
        {
            var dataDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Repository"));
            var morphs  = new MorphCollection();

            foreach (var dir in dataDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                foreach (var file in dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
                {
                    var recipe = Recipes.RecipeParser.Parse(file.FullName);
                    var morph = new Morph(recipe);
                    morphs.Add(morph);
                    Terminal.WriteLine("added ", morph.Tag);
                }
            }

            return morphs;
        }

        public IEnumerable<string> Upstreams
        {
            get
            {
                foreach (var morph in this)
                {
                    foreach (var upstream in morph.Recipe.Upstream)
                    {
                        yield return upstream;
                    }
                }
            }
        }

        public Morph this[int index] => this.list[index];

        public Morph this[string key] => this.lookup[key];

        public int Count => this.list.Count;

        public IEnumerable<string> Keys => this.lookup.Keys;

        public IEnumerable<Morph> Values => this.lookup.Values;

        public bool ContainsKey(string key)
        {
            return this.lookup.ContainsKey(key);
        }

        public bool TryGetValue(string key, out Morph value)
        {
            return this.lookup.TryGetValue(key, out value);
        }

        public IEnumerator<Morph> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, Morph>> IEnumerable<KeyValuePair<string, Morph>>.GetEnumerator()
        {
            return this.lookup.GetEnumerator();
        }
    }
}