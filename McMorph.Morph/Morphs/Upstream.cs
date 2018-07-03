using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using McMorph.Tools;
using McMorph.Files;

using McMorph.Downloads;
using McMorph.Recipes;
using McMorph.Processes;

namespace McMorph.Morphs
{
    public class Upstream
    {
        private readonly Morph morph;
        private readonly Uri uri;

        public Upstream(Morph morph, string uri)
        {
            this.morph = morph;
            this.uri = new Uri(uri);
        }

        public Pogo Pogo => this.Morph.Pogo;
        public Morph Morph => this.morph;


        public bool Extract(bool force)
        {
            var archivePath = Pogo.ArchivePath(this.uri);
            var extractPath = Pogo.SourcePath(this.uri);
            if (!archivePath.ExistsFile())
            {
                throw FilesError.FileNotFoundException(archivePath);
            }
            if (extractPath.ExistsDirectory())
            {
                if (!force)
                {
                    return true;
                }
                Terminal.Write("clean ", extractPath.Name, ": ");
                FileSystemTools.RemoveDirectory(extractPath);
                Terminal.ClearLine();
            }
            if (extractPath.ExistsFile())
            {
                throw FilesError.NewExistsButIsNotDirectory(extractPath);
            }
            PathName extractPathTmp = extractPath.Full + ".tmp";

            if (extractPathTmp.ExistsDirectory())
            {
                Terminal.Write("clean ", extractPathTmp.Name, ": ");
                FileSystemTools.RemoveDirectory(extractPathTmp);
                Terminal.ClearLine();
            }
            if (extractPathTmp.ExistsFile())
            {
                throw FilesError.NewExistsButIsNotDirectory(extractPathTmp);
            }

            extractPathTmp.CreateDirectory();

            Terminal.Write("extract ", archivePath.Name, ": ");

            var xtract = Bash.TarExtract(archivePath, extractPathTmp);

            Terminal.ClearLine();

            var dirs = extractPathTmp.EnumerateDirectories().ToList();
            var files = extractPathTmp.EnumerateFiles().ToList();
            if (dirs.Count == 1 && files.Count == 0)
            {
                dirs.First().MoveDirectory(extractPath);
                extractPathTmp.DeleteDirectory();
            }
            else
            {
                extractPathTmp.MoveDirectory(extractPath);
            }

            Terminal.WriteLine("extract ", xtract.Ok ? "OK: " : "FAIL: ",  archivePath.Name);

            return xtract.Ok;
        }

        public void Download(bool force)
        {
            var downloader = new Downloader();

            var filepath = this.Pogo.ArchivePath(this.uri);

            if (force)
            {
                if (filepath.ExistsFile())
                {
                    filepath.DeleteFile();
                }
                else if (filepath.ExistsDirectory())
                {
                    FileSystemTools.RemoveDirectory(filepath);
                }
            }

            if (filepath.ExistsDirectory())
            {
                throw FilesError.NewExistsButIsNotFile(filepath);
            }

            if (!filepath.ExistsFile())
            {
                var bytes = downloader.GetBytes(this.uri);

                if (bytes != null)
                {
                    filepath.Parent.CreateDirectory();
                    filepath.WriteAllBytes(bytes);
                }
            }
        }
    }
}