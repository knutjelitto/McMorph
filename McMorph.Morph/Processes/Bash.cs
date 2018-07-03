using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using McMorph.Tools;
using McMorph.Files;

namespace McMorph.Processes
{
    public partial class Bash
    {
        private Process process;
        private List<Output> output;
        private string command = null;
        private PathName directory;
        private IReadOnlyDictionary<string, string> environment;
        private Action<IDictionary<string, string>> environmentSetup;
        private bool newEnvironment;
        private Progress progress = null;
        private bool loud = false;
        private bool interactive = false;

        private TextReader input;
        public Bash()
        {
        }

        public int ExitCode => this.process.ExitCode;

        public bool Ok => ExitCode == 0;

        public Bash Command(string command)
        {
            this.command = command;
            return this;
        }

        public Bash Directory(PathName directory)
        {
            this.directory = directory;
            return this;
        }

        public Bash WithProgress(Progress progress = null)
        {
            this.progress = progress ?? new Progress();
            return this;
        }

        public Bash Loud()
        {
            this.loud = true;
            return this;
        }

        public Bash Interactive()
        {
            this.interactive = true;
            return this;
        }

        public Bash WithEnviroment(IReadOnlyDictionary<string, string> environment)
        {
            this.environmentSetup = null;
            this.environment = environment;
            this.newEnvironment = false;
            return this;
        }

        public Bash WithNewEnviroment(IReadOnlyDictionary<string, string> environment)
        {
            this.environmentSetup = null;
            this.environment = environment;
            this.newEnvironment = true;
            return this;
        }

        public Bash WithEnviroment(Action<IDictionary<string, string>> environmentSetup)
        {
            this.environment = null;
            this.environmentSetup = environmentSetup;
            return this;
        }

        public Bash WithNewEnviroment(Action<IDictionary<string, string>> environmentSetup)
        {
            this.environment = null;
            this.newEnvironment = true;
            this.environmentSetup = environmentSetup;
            return this;
        }

        public Bash StdIn(TextReader input)
        {
            this.input = input;
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
            startInfo.RedirectStandardInput = this.input != null;
            startInfo.RedirectStandardOutput = !interactive;
            startInfo.RedirectStandardError = !interactive ;
            
            if (this.directory != null)
            {
                startInfo.WorkingDirectory = this.directory.Full;
            }

            if (this.environment != null)
            {
                if (this.newEnvironment)
                {
                    startInfo.Environment.Clear();
                }
                foreach (var kv in this.environment)
                {
                    startInfo.Environment.Add(kv);
                }
            }
            else if (this.environmentSetup != null)
            {
                if (this.newEnvironment)
                {
                    startInfo.Environment.Clear();
                }
                this.environmentSetup(startInfo.Environment);
            }

            this.output = new List<Output>();
            this.process = new Process() { StartInfo = startInfo };

            if (!this.interactive)
            {
                process.EnableRaisingEvents = !this.interactive;
                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        AddOutput(new StdOut(e.Data));
                    }
                };
                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        AddOutput(new StdErr(e.Data));
                    }
                };
            }

            process.Start();
            if (!this.interactive)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            if (this.input != null)
            {
                string line;
                while ((line = this.input.ReadLine()) != null)
                {
                    process.StandardInput.WriteLine(line);
                    process.StandardInput.Flush();
                }
            }

            process.WaitForExit();

            return this;
        }

        public IEnumerable<Output> Outputs => this.output;
        public IEnumerable<string> AllLines => this.output.Select(o => o.Line);
        public IEnumerable<string> ErrLines => this.output.Where(o => o is StdErr).Select(o => o.Line);
        public IEnumerable<string> OutLines => this.output.Where(o => o is StdOut).Select(o => o.Line);

        private void AddOutput(Output output)
        {
            this.output.Add(output);
            if (this.loud)
            {
                Terminal.WriteLine(output);
            }
            this.progress?.Advance();
        }
        
        public static void ThrowOnError(Bash bash)
        {
            if (!bash.Ok)
            {
                throw Errors.NewProcessError(bash.command, bash.ErrLines.Take(1).ToList(), bash.ExitCode);
            }
        }
    }
}
