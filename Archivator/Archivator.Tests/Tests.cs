using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Archivator.Tests
{
    public class Tests
    {
        [Test]
        public void Text()
        {
            var location = Assembly
                .GetExecutingAssembly()
                .Location
                .Replace("Archivator.Tests", "Archivator")
                .Replace("Archivator.dll", "");

            var inputString = File
                .OpenText(location + "/file.txt")
                .ReadToEnd();

            //Program.Compress(location + "file.txt", location + "archived.txt");
            //Program.DeCompress(location + "archived.txt", location + "deCompressed.txt");

            Program.HCompress(location + "file.txt", location + "archived.txt");
            Program.HDeCompress(location + "archived.txt", location + "deCompressed.txt");

            var outputString = File
                .OpenText(location + "deCompressed.txt")
                .ReadToEnd();
            
            Assert.AreEqual(inputString, outputString);
        }
    }
}