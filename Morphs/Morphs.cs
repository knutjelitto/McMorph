using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using McMorph.Files;

namespace McMorph.Morphs
{
    public class Morphs : IReadOnlyList<Morph>, IReadOnlyDictionary<string, Morph>
    {
        private readonly Pogo pogo;
        private readonly List<Morph> list = new List<Morph>();
        private readonly Dictionary<string, Morph> lookup = new Dictionary<string, Morph>();

        public Morphs(Pogo pogo)
        {
            this.pogo = pogo;
        }

        public static Morphs Populate(Pogo pogo, UPath dataDir)
        {
            var morphs = new Morphs(pogo);

            foreach (var file in dataDir.AsDirectory.EnumerateFiles("*"))
            {
                if (file.Name.StartsWith("."))
                    continue;

                var recipe = Recipes.RecipeParser.Parse(file.Path.FullName);
                var morph = new Morph(pogo, recipe);
                morphs.Add(morph);
            }

            return morphs;
        }

        protected void Add(Morph morph)
        {
            this.lookup.Add(morph.Name, morph);
            this.list.Add(morph);
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

        public IEnumerable<Asset> Assets
        {
            get
            {
                foreach (var morph in this)
                {
                    foreach (var asset in morph.Assets)
                    {
                        yield return asset;
                    }
                }
            }
        }

        public Morph this[int index] => this.list[index];

        public Morph this[string key] => this.lookup[key];

        public int Count => this.list.Count;

        public IEnumerable<string> Keys => this.lookup.Keys;

        public IEnumerable<Morph> Values => this.lookup.Values;

        public void Download(bool force)
        {
            foreach (var morph in this)
            {
                morph.Upstream.Download(force);
            }
        }

        public bool Extract(bool force)
        {
            foreach (var morph in this)
            {
                var result = morph.Upstream.Extract(force);
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