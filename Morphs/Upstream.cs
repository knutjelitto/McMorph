using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using McMorph.Downloads;
using McMorph.Recipes;
using McMorph.Files;
using McMorph.Processes;
using McMorph.Results;

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


        public bool Extract(bool force)
        {
            var archivePath = Pogo.ArchivePath(this.uri);
            var extractPath = Pogo.SourcePath(this.uri);
            if (!archivePath.Exists)
            {
                if (!archivePath.AsFile.Exists)
                {
                    throw Error.NewFileNotFoundException(archivePath);
                }
                throw Error.NewExistsButIsNotFile(archivePath);
            }
            if (extractPath.AsDirectory.Exists)
            {
                if (!force)
                {
                    return true;
                }
                Terminal.Write("clean ", extractPath.GetName(), ": ");
                FileSystemTools.RemoveDirectory(extractPath);
                Terminal.ClearLine();
            }
            if (extractPath.Exists)
            {
                throw Error.NewExistsButIsNotDirectory(extractPath);
            }
            var extractPathTmp = (UPath)(extractPath.FullName + ".tmp");

            if (extractPathTmp.AsDirectory.Exists)
            {
                Terminal.Write("clean ", extractPathTmp.GetName(), ": ");
                FileSystemTools.RemoveDirectory(extractPathTmp);
                Terminal.ClearLine();
            }
            if (extractPathTmp.Exists)
            {
                throw Error.NewExistsButIsNotDirectory(extractPathTmp);
            }

            extractPathTmp.CreateDirectory();

            Terminal.Write("extract ", archivePath.GetName(), ": ");

            var xtract = Bash.TarExtract(archivePath, extractPathTmp);

            Terminal.ClearLine();

            var dirs = extractPathTmp.AsDirectory.EnumerateDirectories().ToList();
            var files = extractPathTmp.AsDirectory.EnumerateFiles().ToList();
            if (dirs.Count == 1 && files.Count == 0)
            {
                dirs.First().MoveTo(extractPath);
                extractPathTmp.AsDirectory.Delete();
            }
            else
            {
                extractPathTmp.AsDirectory.MoveTo(extractPath);
            }

            Terminal.WriteLine("extract ", xtract.Ok ? "OK: " : "FAIL: ",  archivePath.GetName());

            return xtract.Ok;
        }

        public void Download(bool force)
        {
            var downloader = new Downloader();

            var filepath = this.Pogo.ArchivePath(this.uri);

            var file = filepath.AsFile;

            if (force)
            {
                if (file.Exists)
                {
                    file.Delete();
                }
                else if (filepath.AsDirectory.Exists)
                {
                    FileSystemTools.RemoveDirectory(filepath);
                }
            }

            if (filepath.Exists && !file.Exists)
            {
                throw Error.NewExistsButIsNotFile(filepath);
            }

            if (!file.Exists)
            {
                var bytes = downloader.GetBytes(this.uri);

                if (bytes != null)
                {
                    file.Parent.Create();
                    file.WriteAllBytes(bytes);
                }
            }
        }
    }
}