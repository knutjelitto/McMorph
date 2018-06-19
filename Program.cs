using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Results;
using McMorph.Recipes;
using McMorph.Downloads;

namespace McMorph
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();
            var morphs = ReadMorphs();
            watch.Stop();
            Console.WriteLine("elapsed: {0}", watch.Elapsed);

            Download(morphs.Values);

            Console.Write("any key ...");
            Console.ReadKey();
        }

        static Dictionary<string, Recipe> ReadMorphs()
        {
            var morphDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Morphs"));
            var morphs = new Dictionary<string, Recipe>();

            foreach (var file in morphDir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
            {
                Console.WriteLine("reading {0}", file.FullName);
                var recipe = Recipes.RecipeParser.Parse(file.FullName);
                recipe.Dump(Console.Out);
                morphs.Add(recipe.Name, recipe);
            }

            return morphs;
        }

        static void Download(IEnumerable<Recipe> morphs)
        {
            foreach (var morph in morphs)
            {
                var task = Handle(morph);

                task.Wait();
            }
        }

        static async Task Handle(Recipe recipe)
        {
            foreach (var upstream in recipe.Upstream)
            {
                var downloader = new Downloader();

                var bytes = await downloader.GetBytes(upstream);
            }
        }
    }
}
