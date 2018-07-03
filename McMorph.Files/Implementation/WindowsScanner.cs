using System.Collections.Generic;

namespace McMorph.Files.Implementation
{
    internal class WindowsScanner : PathScanner
    {
        public WindowsScanner(string path) : base(path)
        {}
        
        protected override bool CurrentIsSep()
        {
            return CurrentIs('/') || CurrentIs('\\');
        }

        protected override LeadSegment First()
        {
            if (NextIs(':'))
            {
                if (CurrentIs(ch => 'a' <= ch && ch <='z' || 'A' <= ch && ch <= 'Z'))
                {
                    var drive = this.path[this.current].ToString();
                    Advance();
                    Advance();
                    if (CurrentIs('/') || CurrentIs('\\'))
                    {
                        Advance();
                        while (CurrentIsSep()) // eat all extra separators
                        {
                            Advance();
                        }
                        return new WindowsLeadSegment(drive, true);
                    }
                    return new WindowsLeadSegment(drive, false);
                }
            }
            else if (CurrentIsSep())
            {
                Advance();

                while (CurrentIsSep()) // eat all extra separators
                {
                    Advance();
                }
                return new WindowsLeadSegment(string.Empty, true);
            }
            return new WindowsLeadSegment(string.Empty, false);
        }

    }
}