using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeCompressor.Algorithms
{
    /// <summary>
    /// Класс для разжатия по lzw
    /// </summary>
    public static class LZWDeCompressor
    {
        /// <summary>
        /// Разжимает полученный с помощью алгоритма Хаффмана результат, исходный словарь и записываем в необходимый файл
        /// </summary>
        public static void DeCompress(string deCompressedFilePath, string huffmanResult, string lzwDict)
        {
            var decodeDict = new Dictionary<int, string>(); //динамически заполняемый словарь для декодирования
            StringBuilder decodeResult = new StringBuilder(); //итоговый результат
            int[] decodeCodes; //массив кодов

            var encodedResult = huffmanResult.Split(' '); //нужно для заполнения массива кодов

            foreach (var value in lzwDict)
                decodeDict[decodeDict.Count] = value.ToString(); //заполнение словаря изначальными даннымим

            int quantity = int.Parse(encodedResult[0]); //количество кодов

            decodeCodes = new int[quantity];

            for (int i = 1; i < encodedResult.Length; i++)
                decodeCodes[i - 1] = int.Parse(encodedResult[i]); //заполнение массива кодов

            var previousCode = decodeCodes[0]; //переменная для хранения предыдущего кода
            var firstPhrase = decodeDict[previousCode]; //первая фраза

            decodeResult.Append(firstPhrase); //запись самой первой фразы (нужно по алгоритму)

            for (int i = 1; i < decodeCodes.Length; i++) //проходимся по каждому коду
            {
                var currentCode = decodeCodes[i]; //получаем текущий код

                if (decodeDict.ContainsKey(currentCode)) //проверяем, содержится ли в динамически заполняемом словаре
                //текущий код, если да, то
                {
                    var currentPhrase = decodeDict[currentCode]; //получаем фразу по текущему коду
                    decodeResult.Append(currentPhrase); //добавляем в результат

                    var previousPhrase = decodeDict[previousCode]; //получаем предыдущую фразу через предыдущий код
                    decodeDict[decodeDict.Count] = previousPhrase + currentPhrase[0]; //добавляем в словарь новое значение
                }
                else //если нет, то
                {
                    var currentPhrase = decodeDict[previousCode]; //получаем фразу по предыдущему коду
                    currentPhrase += currentPhrase[0]; //добавляем к фразе последний символ той же фразы

                    decodeDict[decodeDict.Count] = currentPhrase; //добавляем в словарь такую фразу
                    decodeResult.Append(currentPhrase); //добавляем в результат
                }
                previousCode = currentCode; //изменяем предыдущий код на текущий
            }

            File.WriteAllText(deCompressedFilePath, decodeResult.ToString()); //записываем в файл результат
        }
    }
}