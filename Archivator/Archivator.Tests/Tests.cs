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
        private string _inputFilePath;
        private string _archivedFilePath;
        private string _outputFilePath;

        [SetUp]
        public void Setup()
        {
            _inputFilePath = Data.InputFilePath;
            _archivedFilePath = Data.ArchivedFilePath;
            _outputFilePath = Data.OutputFilePath;
        }
        
        [Test]
        public void HuffmanTest()
        {
            HuffmanCompressor.Compress(_inputFilePath, _archivedFilePath);
            HuffmanDeCompressor.DeCompress(_archivedFilePath, _outputFilePath);
            CompareFiles();
        }

        [Test]
        public void LzwTest()
        {
            LZWCompressor.Compress(_inputFilePath, _archivedFilePath);
            LZWDeCompressor.DeCompress(_archivedFilePath, _outputFilePath);
            CompareFiles();
        }

        private void CompareFiles()
        {
            var inputString = File
                .OpenText(_inputFilePath)
                .ReadToEnd();
            
            var outputString = File
                .OpenText(_outputFilePath)
                .ReadToEnd();

            Assert.AreEqual(inputString, outputString);
        }
    }
}