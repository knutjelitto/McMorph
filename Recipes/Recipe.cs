namespace McMorph.Recipes
{
    public class Recipe
    {
        public string Name { get; internal set; }
        public string Version { get; internal set; }

        public string Title { get; internal set; }
        public string Description { get; internal set; }

        public string[] Home { get; internal set; }

        public string[] Deps { get; internal set; }

        public string[] Upstream { get; internal set; }

        public IBuild Build { get; internal set; }
    }
}