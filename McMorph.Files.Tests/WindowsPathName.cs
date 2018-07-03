using System;
using System.Diagnostics;
using Xunit;

using McMorph.Files;

namespace McMorph.Files.Tests
{
    public class WindowsPathNameTests
    {
        [Theory]
        // absolute
        [InlineData(@"c:\", @"c:\")]
        [InlineData(@"c:\xxx", @"c:\xxx")]
        [InlineData(@"c:\xxx", @"c:\xxx\.")]
        [InlineData(@"c:\xxx", @"c:\xxx\.\.\.\.\.")]
        [InlineData(@"c:\xxx\yyy", @"c:\xxx\yyy")]
        [InlineData(@"c:\xxx\yyy", @"c:\xxx\.\yyy")]
        [InlineData(@"c:\xxx\yyy", @"c:\xxx\.\.\.\.\.\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\..\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\..\.\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\.\..\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\..\.\.\.\.\.\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\.\.\.\.\.\..\yyy")]
        [InlineData(@"c:\xxx\..\yyy", @"c:\xxx\.\.\..\.\.\.\yyy")]
        // relative
        [InlineData(@"c:", @"c:")]
        [InlineData(@"c:xxx", @"c:xxx")]
        [InlineData(@"c:xxx", @"c:xxx\.")]
        [InlineData(@"c:xxx", @"c:xxx\.\.\.\.\.")]
        [InlineData(@"c:xxx\yyy", @"c:xxx\yyy")]
        [InlineData(@"c:xxx\yyy", @"c:xxx\.\yyy")]
        [InlineData(@"c:xxx\yyy", @"c:xxx\.\.\.\.\.\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\..\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\..\.\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\.\..\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\..\.\.\.\.\.\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\.\.\.\.\.\..\yyy")]
        [InlineData(@"c:xxx\..\yyy", @"c:xxx\.\.\..\.\.\.\yyy")]
        public void WindowsPathNameWithDrive(string expected, string given)
        {
            Assert.Equal(expected, PathName.WindowsPath(given).ToString());
        }

        [Theory]
        [InlineData(@"", @"c:\")]
        [InlineData(@"xxx", @"c:\xxx")]
        [InlineData(@"xxx", @"c:\xxx\.")]
        [InlineData(@"xxx", @"c:\xxx\.\.\.\.\.")]
        [InlineData(@"yyy", @"c:\xxx\yyy")]
        [InlineData(@"yyy", @"c:\xxx\.\yyy")]
        [InlineData(@"yyy", @"c:\xxx\.\.\.\.\.\yyy")]
        [InlineData(@"yyy", @"c:\xxx\..\yyy")]
        [InlineData(@"yyy", @"c:\xxx\..\.\yyy")]
        [InlineData(@"yyy", @"c:\xxx\.\..\yyy")]
        [InlineData(@"yyy", @"c:\xxx\..\.\.\.\.\.\yyy")]
        [InlineData(@"yyy", @"c:\xxx\.\.\.\.\.\..\yyy")]
        [InlineData(@"yyy", @"c:\xxx\.\.\..\.\.\.\yyy")]
        [InlineData(@"", @"c:")]
        [InlineData(@"xxx", @"c:xxx")]
        [InlineData(@"xxx", @"c:xxx\.")]
        [InlineData(@"xxx", @"c:xxx\.\.\.\.\.")]
        [InlineData(@"yyy", @"c:xxx\yyy")]
        [InlineData(@"yyy", @"c:xxx\.\yyy")]
        [InlineData(@"yyy", @"c:xxx\.\.\.\.\.\yyy")]
        [InlineData(@"yyy", @"c:xxx\..\yyy")]
        [InlineData(@"yyy", @"c:xxx\..\.\yyy")]
        [InlineData(@"yyy", @"c:xxx\.\..\yyy")]
        [InlineData(@"yyy", @"c:xxx\..\.\.\.\.\.\yyy")]
        [InlineData(@"yyy", @"c:xxx\.\.\.\.\.\..\yyy")]
        [InlineData(@"yyy", @"c:xxx\.\.\..\.\.\.\yyy")]
        public void WindowsPathNameWithDriveName(string expected, string given)
        {
            Assert.Equal(expected, PathName.WindowsPath(given).Name);
        }
    }
}
