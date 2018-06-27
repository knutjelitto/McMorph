using System;
using System.Collections.Generic;

using McMorph.Files;

namespace McMorph.Processes
{
    public partial class Bash
    {
        public static Bash Checker()
        {
            var bash = new Bash()
                .Command($"env")
                .WithEnviroment(env =>
                {
                    env["PATH"] = "/bin:/usr/bin:/root/LiFo/bin";
                    env.Remove("PROMPT_COMMAND");
                    env.Remove("PS1");
                })
                .Run();

            bash.Run();

            foreach (var output in bash.Outputs)
            {
                Terminal.WriteLine(output);
            }

            return bash;
        }

    }
}