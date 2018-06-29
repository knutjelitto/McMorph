using System.Collections.Generic;
using System.IO;

namespace McMorph.Recipes
{
    public class BuildBash : Base, IBuildData
    {
        public List<string> Settings { get; } = new List<string>();
        public List<string> Configure { get; } = new List<string>();
        public List<string> Make { get; } = new List<string>();
        public List<string> Install { get; } = new List<string>();

        public override void Dump(TextWriter writer)
        {
            writer.WriteLine("[Build.Bash]");            
            Multi(writer, ".Settings", Settings);
            Multi(writer, ".Configure", Configure);
            Multi(writer, ".Make", Make);
            Multi(writer, ".Install", Install);
        }
    }
}