using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph.Files
{
    public class UnixFileSystem : FileSystem
    {
        public override void CreateSymbolicLink(UPath link, UPath value, bool force = false)
        {
            if (force)
            {
                var file = link.AsFile;
                if (file.Exists)
                {
                    file.Delete();
                }
            }
 			var retval = Syscall.symlink (value.FullName, link.FullName);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
        }
    }
}