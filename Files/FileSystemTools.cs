using McMorph.Processes;
using McMorph.Results;

namespace McMorph.Files
{
    public class FileSystemTools
    {
        public static void ForceRemove(UPath path)
        {
            if (path.AsFile.Exists)
            {
                path.AsFile.Delete();
            }
            else
            {
                RemoveDirectory(path);
            }
        }
        public static void RemoveDirectory(UPath directory)
        {
            if (!directory.AsDirectory.Exists)
            {
                if (directory.Exists)
                {
                    throw Error.NewExistsButIsNotDirectory(directory);
                }
                throw Error.NewDirectoryNotFoundException(directory);
            }

            using (var progress = new Progress())
            {
                RemoveDirectory(directory, progress);
            }
        }

        private static void RemoveDirectory(UPath directory, Progress progress)
        {
            foreach (var file in directory.AsDirectory.EnumerateFiles())
            {
                file.Delete();
                progress.Advance();
            }

            foreach (var child in directory.AsDirectory.EnumerateDirectories())
            {
                RemoveDirectory(child.Path, progress);
            }

            directory.AsDirectory.Delete(false);
            progress.Advance();
        }
    }
}