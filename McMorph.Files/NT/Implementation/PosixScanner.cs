using System;
using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class PosixScanner : PathScanner
    {
        protected override bool CurrentIsSep()
        {
            return CurrentIs('/');
        }

        protected override LeadSegment First()
        {
            if (CurrentIsSep())
            {
                Advance();
                while (CurrentIsSep())
                {
                    Advance();
                }
                return new PosixLeadSegment(true);
            }
            return new PosixLeadSegment(false);
        }
    }
}