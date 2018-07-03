namespace McMorph.Files
{
    public static class SymlinkExtensions
    {
        public static void SymlinkTo(this PathName path, string value, bool force = false)
        {
            PathName.Operations.SymbolicLink(path, value, force);
        }
    }
}