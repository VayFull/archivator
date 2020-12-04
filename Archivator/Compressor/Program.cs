using Archivator.Core;
using Compressor.Algorithms;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Compressor
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            HuffmanCompressor.Compress(Data.InputFilePath, Data.ArchivedFilePath);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);


            //stopWatch.Start();
            //LZWCompressor.Compress(Data.Location + "file.txt", Data.Location + "archived.txt");
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }
    }
}
