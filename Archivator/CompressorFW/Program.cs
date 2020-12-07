using System;

namespace CompressorFW
{
    class Program
    {
        static void Main(string[] args)
        {
            //Часть ниже отвечает за обработку входных параметров
            string inputFilePath = string.Empty;
            string archivedFilePath = string.Empty;
            switch (args.Length)
            {
                case 2:
                    inputFilePath = args[0];
                    archivedFilePath = args[1];
                    break;
                case 4:
                    switch (args[0])
                    {
                        case "-i":
                            inputFilePath = args[1];
                            archivedFilePath = args[3];
                            break;
                        case "-o":
                            archivedFilePath = args[1];
                            inputFilePath = args[3];
                            break;
                        default:
                            ThrowWrongInput();
                            return;
                    }
                    break;
                default:
                    ThrowWrongInput();
                    return;
            }

            LZWCompressor.Compress(inputFilePath, archivedFilePath);  //запуск lzw алгоритма компрессии
        }

        /// <summary>
        /// Метод для вывода ошибок при неверных входных данных
        /// </summary>
        public static void ThrowWrongInput()
        {
            Console.WriteLine("Ошибка: Запустите .exe файл введя аргументы в виде: -i путь к файлу с содержимым в виде текста " +
                              "-o путь до выходного файла, куда должно быть записано сжатое содержимое");
            Console.ReadLine();
        }
    }
}
