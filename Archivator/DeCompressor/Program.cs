using Archivator.Core;
using DeCompressor.Algorithms;
using System;
using System.Diagnostics;

namespace DeCompressor
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length != 4) return;

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            HuffmanDeCompressor.DeCompress(Data.Location + "archived.txt", Data.Location + "deCompressed.txt");
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            //stopWatch.Start();
            //LZWDeCompressor.DeCompress(Data.Location + "archived.txt", Data.Location + "deCompressed.txt");
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }
    }
}
