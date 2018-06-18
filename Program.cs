using System;
using System.IO;

namespace McMorph
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PWD: {0}", Environment.CurrentDirectory);

            Recipes.RecipeParser.Parse(Path.Combine(Environment.CurrentDirectory, "Morphs", "bash"));
        }
    }
}
