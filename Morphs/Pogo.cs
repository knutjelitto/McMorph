using System;
using System.Text.RegularExpressions;

using McMorph.Morphs;
using McMorph.Files;

namespace McMorph.Morphs
{
    public class Pogo
    {
        public Pogo() : this("/root/Pogo")
        {
        }

        public Pogo(UPath root)
        {
            root.AssertAbsolute();
            Root = root;

            LazyBox = new Lazy<ChrootBox>(() => new ChrootBox(this, Data / "Box"));
        }

        public UPath Root { get; }

        public UPath Data => Root / "Data";
        public UPath Compile => Data / "Compile";
        public UPath Archives => Data / "Archives";
        public UPath Sources => Data / "Sources";

        private Lazy<ChrootBox> LazyBox;
        public ChrootBox Box => this.LazyBox.Value;

        public UPath System => Root / "System";
        public UPath Index => System / "Index";

        public void Dump()
        {
            Terminal.WriteLine("Pogo", ".Root:", Root);
            Terminal.WriteLine("Pogo", ".Data:", Data);
            Terminal.WriteLine("Pogo", ".Compile:", Compile);
            Terminal.WriteLine("Pogo", ".Archives:", Archives);
            Terminal.WriteLine("Pogo", ".Sources:", Sources);
            Terminal.WriteLine("Pogo", ".System:", System);
            Terminal.WriteLine("Pogo", ".Index:", Index);
        }

        public UPath ArchivePath(Uri uri)
        {
            return Archives / (uri.Host + uri.LocalPath);
        }
        
        public UPath SourcePath(Uri uri)
        {
            if (TryGetArchiveStem(uri, out var stem))
            {
                var source = Sources / stem;
                return source;
            }

            throw new ApplicationException($"don't know how to prepare '{uri}': unknown archive type");
        }

        private Regex archiveRegex = new Regex(
            @".*(?<extension>\.((tar\.(gz|xz|lz|bz2))|(tzg)))$",
            RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.ExplicitCapture);

        private bool TryGetArchiveStem(Uri uri, out string stem)
        {
            var name = ((UPath)uri.LocalPath).GetName();
            var match = archiveRegex.Match(name);
            if (match.Success)
            {
                var ex = match.Groups["extension"];
                stem = name.Substring(0, name.Length - ex.Length);
                return true;
            }
            stem = null;
            return false;
        }
    }
}
