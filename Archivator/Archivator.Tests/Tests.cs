using Archivator.Core;
using Compressor.Algorithms;
using DeCompressor.Algorithms;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;

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
        public void AlgorithmsTest()
        {
            LZWCompressor.Compress(_inputFilePath, _archivedFilePath);
            HuffmanDeCompressor.DeCompress(_archivedFilePath, _outputFilePath);
            CompareFiles();

            HuffmanTestWithDifferentFilesLength(25);
        }

        private void HuffmanTestWithDifferentFilesLength(int count)
        {
            for (int i = 1; i < count; i++)
            {
                AlgorithmsCompressDecompressWithLength(i);
            }
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

        private void CompareFiles(string inputFilePath, string outputFilePath)
        {
            var inputString = File
                .OpenText(inputFilePath)
                .ReadToEnd();

            var outputString = File
                .OpenText(outputFilePath)
                .ReadToEnd();

            Assert.AreEqual(inputString, outputString);
        }

        private void AlgorithmsCompressDecompressWithLength(int count)
        {
            var str = CreateStringWithLength(count);
            var newFileLocation = Data.Location + count;
            var newOutputFileLocation = newFileLocation + "out";
            File.WriteAllText(Data.Location + count, str);

            LZWCompressor.Compress(newFileLocation, _archivedFilePath);
            HuffmanDeCompressor.DeCompress(_archivedFilePath, newOutputFileLocation);
            CompareFiles(newFileLocation, newOutputFileLocation);
        }

        private string CreateStringWithLength(int count)
        {
            var result = new StringBuilder();
            var rnd = new Random();

            for (int i = 0; i < count; i++)
            {
                result.Append(rnd.Next(0, 10));
            }

            return result.ToString();
        }
    }
}