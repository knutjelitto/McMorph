using McMorph.Tools;

namespace McMorph.Files
{
    public class PathNameTools
    {
        public static void RemoveDirectory(PathName directory)
        {
            if (!directory.ExistsDirectory())
            {
                if (directory.ExistsFile())
                {
                    throw PathNameErrors.EntityExistsButIsNotDirectory(directory);
                }
                throw PathNameErrors.DirectoryNotFoundException(directory);
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