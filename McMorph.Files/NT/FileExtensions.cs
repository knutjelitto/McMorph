using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace McMorph.Files
{
    public static class FileExtensions
    {
        public static bool ExistsFile(this PathName path)
        {
            return File.Exists(path.Full);
        }

        public static void DeleteFile(this PathName path)
        {
            File.Delete(path.Full);
        }

        public static void TouchFile(this PathName path)
        {
            if (!File.Exists(path.Full))
            {
                using (var stream = File.OpenWrite(path.Full))
                {
                    stream.Close();
                }
            }
            Debug.Assert(File.Exists(path.Full));
            File.SetLastWriteTime(path.Full, DateTime.Now);
        }

        public static void CopyFile(this PathName source, PathName destination)
        {
            File.Copy(source.Full, destination.Full);
        }

        public static void CopyFileFrom(this PathName destination, PathName source)
        {
            File.Copy(source.Full, destination.Full);
        }

        public static void WriteAllBytes(this PathName path, byte[] bytes)
        {
            File.WriteAllBytes(path.Full, bytes);
        }

        public static string[] ReadAllLines(this PathName path)
        {
            return File.ReadAllLines(path.Full);
        }
    }
}