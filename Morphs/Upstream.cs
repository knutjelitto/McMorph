using System;
using System.Threading.Tasks;

using McMorph.Downloads;
using McMorph.Recipes;
using McMorph.FS;

namespace McMorph.Morphs
{
    public class Upstream
    {
        private readonly Morph morph;
        private readonly Uri uri;

        public Upstream(Morph morph, Uri uri)
        {
            this.morph = morph;
            this.uri = uri;
        }

        public Pogo Pogo => this.Morph.Pogo;
        public Morph Morph => this.morph;


        public Task<bool> Extract()
        {
            Terminal.WriteLine("extracting to ", this.Pogo.SourcesPath(this.uri).ToString());
            return Task.FromResult(true);
        }

        public bool Download()
        {
            var downloader = new Downloader();

            var filepath = this.Pogo.ArchivesPath(this.uri);

            var file = filepath.AsFile;

            if (!file.Exists)
            {
                var bytes = downloader.GetBytes(this.uri);

                if (bytes != null)
                {
                    file.Parent.Create();
                    file.WriteAllBytes(bytes);

                    return true;
                }

                return false;
            }
            return true;
        }
    }
}