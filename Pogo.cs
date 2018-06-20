using System;
using System.IO;

namespace McMorph
{
    public static class Pogo
    {
        public static string Root => "/Pogo";

        public static string Data => Path.Combine(Root, "Data");

        public static string DataCompile => Path.Combine(Data, "Compile");

        public static string DataCompileArchives => Path.Combine(DataCompile, "Archives");

        public static string ArchivesPath(Url url)
        {
        }
    }
}