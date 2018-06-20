using System;
using System.IO;

namespace McMorph
{
    public static class Pogo
    {
        public static string Root => "/McMorph";

        public static string Data => Path.Combine(Root, "Data");

        public static string Compile => Path.Combine(Data, "Compile");

        public static string Archives => Path.Combine(Data, "Archives");

        public static string Index => Path.Combine(Root, "Index");

        public static string ArchivesPath(Uri uri)
        {
            return Path.Combine(Archives, uri.Host + uri.LocalPath);
        }
    }
}
