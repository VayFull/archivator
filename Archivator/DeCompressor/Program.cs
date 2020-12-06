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
            //string inputFilePath = "";
            //string archivedFilePath = "";
            //switch (args.Length)
            //{
            //    case 2:
            //        inputFilePath = args[0];
            //        archivedFilePath = args[1];
            //        break;
            //    case 4:
            //        switch (args[0])
            //        {
            //            case "-i":
            //                inputFilePath = args[1];
            //                archivedFilePath = args[3];
            //                break;
            //            case "-o":
            //                archivedFilePath = args[1];
            //                inputFilePath = args[3];
            //                break;
            //            default:
            //                ThrowWrongInput();
            //                return;
            //        }
            //        break;
            //    default:
            //        ThrowWrongInput();
            //        return;
            //}
            
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            HuffmanDeCompressor.DeCompress(Data.ArchivedFilePath, Data.OutputFilePath);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            //stopWatch.Start();
            //LZWDeCompressor.DeCompress(Data.Location + "archived.txt", Data.Location + "deCompressed.txt");
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }
        
        public static void ThrowWrongInput()
        {
            Console.WriteLine("Введите аргументы в виде: -i путь до файла со сжатым содержимым " +
                              "-o путь до файла с результатом декодирования");
            Console.ReadLine();
        }
    }
}
