using System;
using System.Reflection;

namespace McMorph.Processes
{
    public class Self
    {
        public static int Exec()
        {
            var assembly = Assembly.GetEntryAssembly();

            Terminal.WriteLine("codebase: ", assembly.CodeBase);
            Terminal.WriteLine("location: ", assembly.Location);

            return 0;
        }
    }
}