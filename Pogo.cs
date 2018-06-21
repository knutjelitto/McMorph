using System;
using System.IO;

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

        public string Index => Path.Combine(Root, "Index");

        public string ArchivesPath(Uri uri)
        {
            return Path.Combine(Archives, uri.Host + uri.LocalPath);
        }
    }
}
