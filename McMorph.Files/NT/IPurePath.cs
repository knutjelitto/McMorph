using System;
using System.Collections.Generic;

namespace McMorph.Files
{
    public interface IPurePath : IEquatable<IPurePath>, IComparable<IPurePath>
    {
        PathFlawor Flawor { get; }

        string Path { get; }
    }
}