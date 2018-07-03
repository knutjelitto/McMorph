using System;
using System.Diagnostics;
using Xunit;

using McMorph.Files;

namespace McMorph.Files.Tests
{
    public class McPathTests
    {
        [Theory]
        [InlineData(".",       null)]
        [InlineData(".",       "")]
        [InlineData(".",       ".")]
        [InlineData(".",       "./.")]
        [InlineData(".",       "././.")]
        [InlineData(".",       "./")]
        [InlineData(".",       "././")]
        [InlineData(".",       "./././")]
        [InlineData("x",       "x")]
        [InlineData("../x",     "../x")]
        [InlineData("../x",     ".././x")]
        [InlineData("../x",     "../x/.")]
        [InlineData("../../x",  "../.././x")]
        [InlineData("x/y",     "x/y")]
        public void PurePosixPathRelative(string expected, string trialMaterial)
        {
            Assert.Equal(expected, McPath.PurePosixPath(trialMaterial).ToString());
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
        public void PurePosixPathAbsolute(string expected, string trialMaterial)
        {
            Assert.Equal(expected, McPath.PurePosixPath(trialMaterial).ToString());
        }
    }
}
