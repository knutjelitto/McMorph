using System;

using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph.Files.Implementation
{
    public class PosixOperations : ISpecialOperations
    {
        public void SymbolicLink(PathName path, string value, bool force)
        {
            if (force)
            {
                var file = path.ExistsFile();
                if (path.ExistsFile())
                {
                    path.DeleteFile();
                }
            }
 			var retval = Syscall.symlink (value, path.Full);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
        }
    }
}