using System;
using System.Threading.Tasks;

using McMorph.Downloads;
using McMorph.Recipes;
using McMorph.FS;
using McMorph.Tools;

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


        public bool Extract()
        {
            var archivePath = Pogo.ArchivesPath(this.uri);
            var extractPath = Pogo.SourcesPath(this.uri);
            if (!archivePath.AsFile.Exists)
            {
                throw new ApplicationException($"can't extract '{archivePath}': doesn't exists");
            }
            if (extractPath.AsDirectory.Exists)
            {
                return true;
            }
            Terminal.WriteLine("extracting ", archivePath, " to ", extractPath);
            var xtract = new Extract();

            return xtract.Run(archivePath, extractPath);
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