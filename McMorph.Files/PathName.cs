using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using McMorph.Files.Implementation;

namespace McMorph.Files
{
    public abstract class PathName
    {
        static readonly PathParser posixParser;
        static readonly PathParser windowsParser;
        static readonly PathParser defaultParser;
        public static readonly ISpecialOperations Operations;

        static PathName()
        {
            posixParser = new PathParser(path => new PosixScanner(path), (lead, rest) => new PosixPathName(lead, rest));
            windowsParser = new PathParser(path => new WindowsScanner(path), (lead, rest) => new WindowsPathName(lead, rest));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                defaultParser = windowsParser;
                Operations = new WindowsOperations();
            }
            else
            {
                defaultParser = posixParser;
                Operations = new PosixOperations();
            }
        }

        public static PathName Path(string path)
        {
            return defaultParser.Parse(path);
        }

        public static PosixPathName PosixPath(string path)
        {
            return (PosixPathName)posixParser.Parse(path);
        }

        public static WindowsPathName WindowsPath(string path)
        {
            return (WindowsPathName)windowsParser.Parse(path);
        }

        internal readonly LeadSegment lead;
        protected readonly List<Segment> segments;

        internal PathName(LeadSegment lead, IEnumerable<Segment> segments)
        {
            this.lead = lead;
            this.segments = segments.ToList();
        }

        public string Full => this.ToString();

        public string Name
        {
            get
            {
                if (this.segments.Count == 0)
                {
                    return string.Empty;
                }
                return this.segments.LastOrDefault()?.ToString() ?? string.Empty;
            }
        }        

        public PathName Parent => this.lead.Create(this.segments.Take(this.segments.Count - 1));

        public bool IsAbsolute => this.lead.IsAbsolute;

        public bool IsAnchored => this.lead.IsAnchored;

        internal abstract PathName Join(PathName other);

        public static implicit operator PathName(string path)
        {
            return Path(path);
        }

        public static PathName operator/(PathName left, PathName right)
        {
            if (left != null)
            {
                if (right != null) // left != null && right != null;
                {
                    return left.Join(right);
                }
                else // left != null &&  right == null
                {
                    return left;
                }
            }
            else // left == null
            {
                if (right != null) // left == null && right != null
                {
                    return right;
                }
                else // left == null && right == null
                {
                    return PathName.Path(null);
                }
            }
        }

        public override string ToString()
        {
            return this.lead.ToString() + string.Join(this.lead.Separator, this.segments);
        }
    }
}