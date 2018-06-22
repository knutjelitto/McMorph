using System;
using System.IO;
using System.Threading.Tasks;

using McMorph.Downloads;
using McMorph.Recipes;

namespace McMorph.Morphs
{
    public class Upstream
    {
        private readonly Pogo pogo;
        private readonly Morph morph;
        public Upstream(Pogo pogo, Morph morph)
        {
            this.pogo = pogo;
            this.morph = morph;
        }

        public Morph Morph => this.morph;

        public Task Extract()
        {
            return null;
        }

        public async Task Download()
        {
            var downloader = new Downloader();

            var filepath = this.pogo.ArchivesPath(this.morph.Upstream);
            
            if (!File.Exists(filepath))
            {
                try
                {
                    var bytes = await downloader.GetBytes(this.morph.UpstreamValue);

                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                    File.WriteAllBytes(filepath, bytes);
                }
                catch {}
            }
        }
    }
}