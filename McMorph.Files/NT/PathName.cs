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
        [ThreadStatic] static readonly PathParser posixParser;
        [ThreadStatic] static readonly PathParser windowsParser;
        [ThreadStatic] static readonly PathParser defaultParser;
        public static readonly ISpecialOperations Operations;

        static PathName()
        {
            posixParser = new PathParser(new PosixScanner(), (lead, rest) => new PosixPathName(lead, rest));
            windowsParser = new PathParser(new WindowsScanner(), (lead, rest) => new WindowsPathName(lead, rest));
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

        private readonly LeadSegment lead;
        private readonly List<Segment> segments;

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

        public static implicit operator PathName(string path)
        {
            return Path(path);
        }

        public static PathName operator/(PathName left, PathName right)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.lead.ToString() + string.Join(this.lead.Separator, this.segments);
        }
    }
}