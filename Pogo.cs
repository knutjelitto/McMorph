using System;
using System.IO;

using McMorph.Morphs;

namespace McMorph
{
    public class Pogo
    {
        public Pogo() : this("/McMorph")
        {
        }

        public Pogo(string root)
        {
            Root = root;
        }

        public string Root { get; }

        public string Data => Path.Combine(Root, "Data");
        public string Compile => Path.Combine(Data, "Compile");
        public string Archives => Path.Combine(Data, "Archives");
        public string Sources => Path.Combine(Data, "Sources");
        public string Index => Path.Combine(Root, "Index");

        public string ArchivesPath(Upstream upstream)
        {
            var uri = new Uri(upstream.Morph.UpstreamValue);
            return Path.Combine(Archives, uri.Host + uri.LocalPath);
        }

        public string SourcesPath(Upstream upstream)
        {
            var source = Path.Combine(Sources, upstream.Morph.Tag);
            return source;
        }
    }
}
