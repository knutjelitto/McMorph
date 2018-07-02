namespace McMorph.Morphs
{
    public interface IBuilder
    {
        void Configure();
        void Make();
        void Install();
        void Harvest();
    }
}