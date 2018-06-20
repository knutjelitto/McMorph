using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using McMorph.Results;
using McMorph.Recipes;
using McMorph.Downloads;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph
{
    class Program
    {
        static void Main(string[] args)
        {
            var morphs = ReadRecipes();

            //Syscall.chroot(Pogo.DataArchives);

            FindFiles();

            //Exec.Bash();

            Download(morphs.Values);

            Console.Write("any key ...");
            Console.ReadKey();
        }

        static Dictionary<string, Recipe> ReadRecipes()
        {
            var dataDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Repository"));
            var recipes = new Dictionary<string, Recipe>();

            foreach (var dir in dataDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                foreach (var file in dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
                {
                    var recipe = Recipes.RecipeParser.Parse(file.FullName);
                    recipes.Add(recipe.Name, recipe);
                    Host.WriteLine(recipe.Name, "-", recipe.Version);
                }
            }

            return recipes;
        }

        static void Download(IEnumerable<Recipe> morphs)
        {
            var downloader = new Downloader();

            foreach (var morph in morphs)
            {
                foreach (var upstream in morph.Upstream)
                {
                    var filepath = Pogo.ArchivesPath(new Uri(upstream));
                    
                    if (!File.Exists(filepath))
                    {
                        try
                        {
                            var task =  downloader.GetBytes(upstream);

                            var bytes = task.Result;

                            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                            File.WriteAllBytes(filepath, bytes);
                        }
                        catch {}
                    }
                }
            }
        }

        static void FindFiles()
        {
            foreach (var drive in UnixDriveInfo.GetDrives())
            {
                Host.WriteLine("DRIVE: ", drive.Name);
            }
        }
    }
}
