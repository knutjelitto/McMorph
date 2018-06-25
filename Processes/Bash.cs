using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace McMorph.Processes
{
    public class Bash
    {
        private Process process;

        private string command = null;
        private ISource stdIn;
        private ISink stdOut;
        private ISink stdErr;

        public Bash()
        {
        }

        public int ExitCode => this.process.ExitCode;

        public Bash Command(string command)
        {
            this.command = command;

            return this;
        }

        public Bash StdIn(ISource stdIn)
        {
            this.stdIn = stdIn;
            return this;
        }

        public Bash StdOut(ISink stdOut)
        {
            this.stdOut = stdOut;
            return this;
        }

        public Bash StdErr(ISink stdErr)
        {
            this.stdErr = stdErr;
            return this;
        }

        public Bash Run()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = "-c \"" + command.Replace("\"", "\\\"") + "\"";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = this.stdIn != null;
            startInfo.RedirectStandardOutput = this.stdOut != null;
            startInfo.RedirectStandardError = this.stdErr != null;

            this.process = new Process() { StartInfo = startInfo };

            if (startInfo.RedirectStandardOutput)
            {
                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        this.stdOut.PutLine(e.Data);
                    }
                };
                process.EnableRaisingEvents = true;
            }

            if (startInfo.RedirectStandardError)
            {
                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        this.stdErr.PutLine(e.Data);
                    }
                };
                process.EnableRaisingEvents = true;
            }

            process.Start();

            if (startInfo.RedirectStandardOutput)
            {
                process.BeginOutputReadLine();
            }
            if (startInfo.RedirectStandardError)
            {
                process.BeginErrorReadLine();
            }
            if (startInfo.RedirectStandardInput)
            {
                string line;
                while ((line = this.stdIn.GetLine()) != null)
                {
                    process.StandardInput.WriteLine(line);
                    process.StandardInput.Flush();
                }
            }

            process.WaitForExit();

            return this;
        }
    }
}
