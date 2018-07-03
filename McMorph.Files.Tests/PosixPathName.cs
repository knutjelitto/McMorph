using System;
using System.Diagnostics;
using Xunit;

using McMorph.Files;

namespace McMorph.Files.Tests
{
    public class PosixPathNameTests
    {
        [Theory]
        [InlineData("",       null)]
        [InlineData("",       "")]
        [InlineData("",       ".")]
        [InlineData("",       "./.")]
        [InlineData("",       "././.")]
        [InlineData("",       "./")]
        [InlineData("",       "././")]
        [InlineData("",       "./././")]
        [InlineData("x",       "x")]
        [InlineData("../x",     "../x")]
        [InlineData("../x",     ".././x")]
        [InlineData("../x",     "../x/.")]
        [InlineData("../../x",  "../.././x")]
        [InlineData("x/y",     "x/y")]
        public void PosixPathNameRelative(string expected, string trialMaterial)
        {
            Assert.Equal(expected, PathName.PosixPath(trialMaterial).ToString());
        }


        [Theory]
        [InlineData("/", "/")]
        [InlineData("/", "//")]
        [InlineData("/", "/./")]
        [InlineData("/", "/.//")]
        [InlineData("/", "//./")]
        [InlineData("/", "//.//")]
        [InlineData("/", "/././.")]
        [InlineData("/x","/x/")]
        [InlineData("/x","/x//")]
        [InlineData("/x","/x/./")]
        [InlineData("/x","/x/.//")]
        [InlineData("/x","/x//./")]
        [InlineData("/x","/x//.//")]
        [InlineData("/x","/x/././.")]
        public void PosixPathNameAbsolute(string expected, string trialMaterial)
        {
            Assert.Equal(expected, PathName.PosixPath(trialMaterial).ToString());
        }
    }
}
