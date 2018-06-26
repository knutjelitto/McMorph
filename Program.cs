﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Results;
using McMorph.Recipes;
using McMorph.Downloads;
using McMorph.Processes;
using McMorph.Morphs;
using McMorph.Files;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph
{
    class Program
    {
        public static Pogo Pogo;

        static void Main(string[] args)
        {

            Pogo = new Pogo();

            var morphs = MorphCollection.Populate(Pogo);
            Terminal.ClearLine();
            Terminal.WriteLine("reading OK");
            
            morphs.Download();
            morphs.Extract();

            Bash.MountOverlay(Pogo.Box);

            //Console.Write("any key ...");
            //Console.ReadKey(true);
            //Console.WriteLine();
        }
    }
}
