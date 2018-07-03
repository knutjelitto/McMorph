using System;

using McMorph.Tools;

namespace McMorph.Files
{
    public abstract class PurePath : IPurePath
    {
        public PurePath(PathFlawor flawor)
        {
            this.Flawor = flawor;
        }

        public PathFlawor Flawor { get; }

        public abstract string Path { get; }

        public int CompareTo(IPurePath other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IPurePath other)
        {
            throw new NotImplementedException();
        }
    }
}