using System;
using System.Diagnostics;
using Xunit;

using McMorph.Files;

namespace McMorph.Files.Tests
{
    public class McPathTests
    {
        [Fact]
        public void PurePosixPath()
        {
            Assert.Equal(".",       McPath.PurePosixPath("././.").ToString());
            Assert.Equal("x",       McPath.PurePosixPath("x").ToString());
            Assert.Equal("x/y",     McPath.PurePosixPath("x/y").ToString());
            Assert.Equal("../x",    McPath.PurePosixPath("../x").ToString());
            Assert.Equal(".",       McPath.PurePosixPath("./").ToString());
            Assert.Equal(".",       McPath.PurePosixPath("./.").ToString());
            Assert.Equal(".",       McPath.PurePosixPath("././").ToString());
        }
    }
}
