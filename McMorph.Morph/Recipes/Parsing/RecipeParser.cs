using System;
using System.Linq;
using System.Reflection;

using McMorph.Tools;
using McMorph.Files;

namespace McMorph.Recipes
{
    public class RecipeParser
    {
        public static Recipe Parse(PathName filepath)
        {
            var lines = new Lines(filepath.ReadAllLines());

            var recipe = new Recipe();

            Action<string> setter = (_) => {};

            Action<Tag> context = RootContext;

            void RootContext(Tag tag)
            {
                switch (tag.ToString().ToLowerInvariant())
                {
                    case  "title":
                        setter = value => recipe.Title = value;
                        break;
                    case  "description":
                        setter = value => recipe.Description.Add(value);
                        break;
                    case  "home":
                        setter = value => recipe.Home.Add(value);
                        break;
                    case  "name":
                        setter = value => recipe.Name = value;
                        break;
                    case  "version":
                        setter = value => recipe.Version = value;
                        break;
                    case  "upstream":
                        setter = value =>
                        {
                            if (recipe.Upstream != null)
                            {
                                throw new ArgumentException("already set", nameof(recipe.Upstream));
                            }
                            recipe.Upstream = value;
                        };
                        break;
                    case  "assets":
                        setter = value => recipe.Assets.Add(value);
                        break;
                    case  "deps":
                    {
                        setter = value => recipe.Deps.AddRange(value.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                        break;
                    }
                    case "class":
                    {
                        setter = value => 
                        {
                            
                            var options = string.Join(", ", value.Split(' ', '|').Where(s => !string.IsNullOrWhiteSpace(s)));
                            var add = (RecipeClass)Enum.Parse(typeof(RecipeClass), options, true);
                            recipe.Class = recipe.Class | add;
                        };
                        break;
                    }
                    case "build.bash":
                    {
                        recipe.Build = new BuildBash();
                        context = BuildBashContext;
                        break;
                    }
                    default:
                        Errors.Throw($"{filepath}({tag.LineNo}): unknown tag [{tag}]");
                        break;
                }
            }

            void BuildBashContext (Tag tag)
            {
                var text = tag.ToString().ToLowerInvariant();
                switch (text)
                {
                    case  ".settings":
                        setter = value => recipe.Build.Settings.Add(value);
                        break;
                    case  ".configure":
                        setter = value => recipe.Build.Configure.Add(value);
                        break;
                    case  ".make":
                        setter = value => recipe.Build.Make.Add(value);
                        break;
                    case  ".install":
                        setter = value => recipe.Build.Make.Add(value);
                        break;
                    default:
                        if (text.StartsWith("."))
                        {
                            Errors.Throw($"{filepath}({tag.LineNo}): unknown tag [{tag}]");
                        }
                        context = RootContext;
                        RootContext(tag);
                        break;
                }
            }

            foreach (var token in lines)
            {
                if (token is Tag tag)
                {
                    context(tag);
                }
                else if (token is Value val)
                {
                    setter(val.Text);
                }
                else
                {
                    var err = (ErrTok)token;

                    Errors.Throw($"{filepath}({token.LineNo}): {err.Message}");
                }
            }

            recipe.Upstream = recipe.Upstream.Replace("@[Name]", recipe.Name).Replace("@[Version]", recipe.Version);

            return recipe;
        }
    }
}