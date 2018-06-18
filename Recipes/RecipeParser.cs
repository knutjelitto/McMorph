using System;
using System.IO;

namespace McMorph.Recipes
{
    public class RecipeParser
    {
        public static Recipe Parse(string filename)
        {
            var lines = new Lines(File.ReadAllLines(filename));

            Console.WriteLine($"{lines.Count} lines read");

            var recipe = new Recipe();

            var line = lines.First();

            while (line != null)
            {
                if (line.IsTag)
                {
                    if (line.Tags.Count == 1)
                    {
                        
                    }
                }
                line = line.Next;
            }

            return null;
        }
    }
}