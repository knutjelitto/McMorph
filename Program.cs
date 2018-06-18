using System;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;

using McMorph.Results;
using McMorph.Recipes;
using McMorph.Downloads;

namespace McMorph
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PWD: {0}", Environment.CurrentDirectory);

            var morphDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Morphs"));

            foreach (var file in morphDir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
            {
                var recipe = Recipes.RecipeParser.Parse(file.FullName);
                //recipe.Dump(Console.Out);

                var task =  Handle(recipe);

                task.Wait();
            }


            Console.Write("any key ...");
            Console.ReadKey();
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
