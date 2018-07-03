using System;

namespace McMorph.Files.Implementation
{
    public class WindowsOperations : ISpecialOperations
    {
        public void SymbolicLink(PathName path, string value, bool force)
        {
            throw new PlatformNotSupportedException();
        }
    }
}