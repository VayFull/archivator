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
            LZWCompressor.Compress(Data.InputFilePath, Data.ArchivedFilePath);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);
            Console.ReadLine();
            //stopWatch.Start();
            //LZWCompressor.Compress(Data.Location + "file.txt", Data.Location + "archived.txt");
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        public static void ThrowWrongInput()
        {
            Console.WriteLine("Введите аргументы в виде: -i путь к файлу с содержимым в виде текста " +
                              "-o путь до выходного файла, куда должно быть записано сжатое содержимое");
            Console.ReadLine();
        }
    }
}
