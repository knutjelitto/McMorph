using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Files;

namespace McMorph.Processes
{
    public partial class Bash
    {
        private Process process;
        private List<Output> output;
        private string command = null;
        private UPath directory;
        private Dictionary<string, string> environment;
        private Action<IDictionary<string, string>> environmentSetup;
        private Progress progress = null;

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

        public Bash Directory(UPath directory)
        {
            this.directory = directory;
            return this;
        }

        public Bash WithProgress()
        {
            this.progress = new Progress();
            return this;
        }

        public Bash WithEnviroment(IReadOnlyDictionary<string, string> environment, bool merge = false)
        {
            this.environmentSetup = null;
            this.environment = new Dictionary<string, string>();
            if (merge)
            {
                foreach (string key in Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process).Keys)
                {
                    this.environment[key] = Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
                }
            }
            foreach (var kv in environment)
            {
                this.environment[kv.Key] = kv.Value;
            }
            return this;
        }

        public Bash WithEnviroment(Action<IDictionary<string, string>> environmentSetup)
        {
            this.environment = null;
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
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = this.directory != null ? (string)this.directory : string.Empty;

            if (this.environment != null)
            {
                startInfo.Environment.Clear();
                foreach (var kv in this.environment)
                {
                    startInfo.Environment.Add(kv);
                }
            }
            else if (this.environmentSetup != null)
            {
                this.environmentSetup(startInfo.Environment);
            }

            this.output = new List<Output>();
            this.process = new Process() { StartInfo = startInfo };

            process.EnableRaisingEvents = true;
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

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

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

        private void AddOutput(Output output)
        {
            this.output.Add(output);
            if (this.progress != null)
            {
                this.progress.Advance();
            }
        }
    }
}
