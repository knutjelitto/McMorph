using System;
using System.Diagnostics;

namespace McMorph
{
    public static class Exec
    {
        public static void Bash()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"find / -type d\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,

                    //RedirectStandardError = true,
                    //RedirectStandardOutput = true,
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}