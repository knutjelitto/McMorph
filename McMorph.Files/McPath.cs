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
            if (path.StartsWith("./") || path.StartsWith("../"))
            {
                this.uri = new Uri(path, UriKind.Relative);
            }
            else
            {
                this.uri = null;
            }
        }
    }
}