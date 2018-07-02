using Mono.Unix;
using Mono.Unix.Native;

namespace McMorph.Files
{
    public class UnixFileSystem : FileSystem
    {
        public override void SetSticky(UPath path, bool set)
        {
            var fsInfo = UnixFileSystemInfo.GetFileSystemEntry(path.FullName);

            if (set)
            {
                fsInfo.FileSpecialAttributes |= FileSpecialAttributes.Sticky;
            }
            else
            {
                fsInfo.FileSpecialAttributes &= ~FileSpecialAttributes.Sticky;
            }
        }

        public override void SetAllPermissions(UPath path)
        {
            var fsInfo = UnixFileSystemInfo.GetFileSystemEntry(path.FullName);

            fsInfo.FileAccessPermissions = FileAccessPermissions.AllPermissions;
        }

        public override void SetDefaultPermissions(UPath path)
        {
            var fsInfo = UnixFileSystemInfo.GetFileSystemEntry(path.FullName);

            fsInfo.FileAccessPermissions = FileAccessPermissions.DefaultPermissions;
        }

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