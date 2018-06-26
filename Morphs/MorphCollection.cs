using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using McMorph.Files;

namespace McMorph.Morphs
{
    public class MorphCollection : IReadOnlyList<Morph>, IReadOnlyDictionary<string, Morph>
    {
        private readonly Pogo pogo;
        private readonly List<Morph> list = new List<Morph>();
        private readonly Dictionary<string, Morph> lookup = new Dictionary<string, Morph>();

        public MorphCollection(Pogo pogo)
        {
            this.pogo = pogo;
        }

        public void Add(Morph morph)
        {
            this.lookup.Add(morph.Name, morph);
            this.list.Add(morph);
        }

        public static MorphCollection Populate(Pogo pogo)
        {
            var dataDir = (UPath)Environment.CurrentDirectory / "Repository";
            var morphs = new MorphCollection(pogo);

            foreach (var file in dataDir.AsDirectory.EnumerateFiles("*"))
            {
                if (file.Name.StartsWith("."))
                    continue;

                var recipe = Recipes.RecipeParser.Parse(file.FullName);
                var morph = new Morph(pogo, recipe);
                morphs.Add(morph);
                Terminal.ClearLine();
                Terminal.Write("reading OK: ", morph.Tag);
            }

            return morphs;
        }

        public IEnumerable<Upstream> Upstreams
        {
            get
            {
                foreach (var morph in this)
                {
                        yield return morph.Upstream;
                }
            }
        }

        public Morph this[int index] => this.list[index];

        public Morph this[string key] => this.lookup[key];

        public int Count => this.list.Count;

        public IEnumerable<string> Keys => this.lookup.Keys;

        public IEnumerable<Morph> Values => this.lookup.Values;

        public bool Download()
        {
            foreach (var morph in this)
            {
                var result = morph.Upstream.Download();
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Extract()
        {
            foreach (var morph in this)
            {
                var result = morph.Upstream.Extract();
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

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