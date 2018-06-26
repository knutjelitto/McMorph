using System;
using System.Text.RegularExpressions;

using McMorph.Morphs;
using McMorph.Files;

namespace McMorph
{
    public class Pogo
    {
        public Pogo() : this("/Pogo")
        {
        }

        public Pogo(UPath root)
        {
            root.AssertAbsolute();
            Root = root;

            LazyBox = new Lazy<TheBox>(() => new TheBox(Data / "box"));
        }

        private UPath Root { get; }

        public UPath Data => Root / "Data";
        public UPath Compile => Data / "Compile";
        public UPath Archives => Data / "Archives";
        public UPath Sources => Data / "Sources";

        private Lazy<TheBox> LazyBox;
        public TheBox Box => this.LazyBox.Value;

        public UPath System => Root / "System";
        public UPath Index => System / "Index";

        public class TheBox
        {
            private readonly UPath root;

            public TheBox(UPath root)
            {
                this.root = root;
            }
            public UPath Changes => this.root / "Changes";
            public UPath Work => this.root / "Work";
            public UPath Residuum => this.root / "Residuum";
            public UPath Root => this.root / "Root";
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
