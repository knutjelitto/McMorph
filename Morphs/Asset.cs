using System;

namespace McMorph.Morphs
{
    public class Asset
    {
        private readonly Morph morph;
        private readonly Uri uri;

        public Asset(Morph morph, string uri)
        {
            this.morph = morph;
            this.uri = new Uri(uri);
        }
    }
}