using McMorph.Tools;

namespace McMorph.Files
{
    public class FileSystemTools
    {
        public static void RemoveDirectory(PathName directory)
        {
            if (!directory.ExistsDirectory())
            {
                if (directory.ExistsFile())
                {
                    throw FilesError.NewExistsButIsNotDirectory(directory);
                }
                throw FilesError.NewDirectoryNotFoundException(directory);
            }

            using (var progress = new Progress())
            {
                RemoveDirectory(directory, progress);
            }
        }

        private static void RemoveDirectory(PathName directory, Progress progress)
        {
            foreach (var file in directory.EnumerateFiles())
            {
                file.DeleteFile();
                progress.Advance();
            }

            foreach (var child in directory.EnumerateDirectories())
            {
                RemoveDirectory(child, progress);
            }

            directory.DeleteDirectory();
            progress.Advance();
        }
    }
}