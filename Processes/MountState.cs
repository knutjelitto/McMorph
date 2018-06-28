using System;
using System.Collections.Generic;

namespace McMorph.Processes
{
    public class MountState
    {
        public readonly Stack<MountPoint> MountPoints = new Stack<MountPoint>();

        public MountState()
        {
        }

        public void Add(MountPoint mountPoint)
        {
            MountPoints.Push(mountPoint);
        }
    }
}