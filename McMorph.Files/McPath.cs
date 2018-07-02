using System;

namespace McMorph.Files
{
    public struct McPath
    {
        private readonly Uri uri;

        public McPath(string path) : this(path, false)
        {
        }

        internal McPath(string path, bool validated)
        {
            this.uri = new Uri(path);
        }
    }
}