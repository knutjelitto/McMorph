using System;
using System.Collections.Generic;

namespace McMorph.Processes
{
    public class CollectSink : ISink
    {
        private readonly List<string> lines = new List<string>();

        public void PutLine(string line)
        {
            lines.Add(line);
        }
    }
}