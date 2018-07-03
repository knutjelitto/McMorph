namespace McMorph.Files
{
    public interface ISpecialOperations
    {
         void SymbolicLink(PathName path, string value, bool force = false);
    }
}