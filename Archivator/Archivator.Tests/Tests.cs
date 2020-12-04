using System.IO;
using System.Reflection;
using Archivator.Core;
using Compressor.Algorithms;
using DeCompressor.Algorithms;
using NUnit.Framework;

namespace Archivator.Tests
{
    public class Tests
    {
        [Test]
        public void Text()
        {
            var location = Data.Location;

            var inputString = File
                .OpenText(location + "/file.txt")
                .ReadToEnd();

            HuffmanCompressor.Compress(location + "file.txt", location + "archived.txt");
            HuffmanDeCompressor.DeCompress(location + "archived.txt", location + "deCompressed.txt");

            //LZWCompressor.Compress(location + "file.txt", location + "archived.txt");
            //LZWDeCompressor.DeCompress(location + "archived.txt", location + "deCompressed.txt");

            var outputString = File
                .OpenText(location + "deCompressed.txt")
                .ReadToEnd();

            Assert.AreEqual(inputString, outputString);
        }
    }
}