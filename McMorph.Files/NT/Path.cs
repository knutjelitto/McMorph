namespace McMorph.Files.NT
{
    public abstract class Path : PurePath, IPath
    {
        public Path(PathFlawor flawor) : base(flawor)
        {
        }
    }
}