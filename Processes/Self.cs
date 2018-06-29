using System;
using System.Reflection;
using System.Diagnostics;

namespace McMorph.Processes
{
    public class Self
    {
        public static int Exec()
        {
            var assembly = Assembly.GetEntryAssembly();

            var startInfo = new ProcessStartInfo()
            {
                FileName = "/usr/bin/dotnet",
                Arguments = $"{assembly.Location} {Program.ChrootIntro}",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
            };

            var process = new Process() { StartInfo = startInfo };

            process.Start();

            process.WaitForExit();

            return process.ExitCode;
        }
    }
}
