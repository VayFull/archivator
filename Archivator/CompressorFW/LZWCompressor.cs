﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompressorFW
{
    /// <summary>
    /// Класс для сжатия по lzw
    /// </summary>
    public static class LZWCompressor
    {
        /// <summary>
        /// Сжимает файл fileToCompressPath, передаёт алфавит и последовательность кодов алгоритму сжатия по Хаффману 
        /// </summary>
        /// <param name="fileToCompressPath">путь до файла, который необходимо сжать</param>
        /// <param name="compressedFilePath">путь до файла, в который нужно записать результат сжатия</param>
        public static void Compress(string fileToCompressPath, string compressedFilePath)
        {
            var dict = new Dictionary<string, int>(); //динамически заполняемый словарь
            var inputString = File
                .OpenText(fileToCompressPath)
                .ReadToEnd(); //исходный текст

            var distinctedInputString = inputString.Distinct(); //все уникальные символы текста
            //нужно для заполнения изначального словаря

            StringBuilder dictionary = new StringBuilder(); //изначальный словарь, сделан строкой, т.к этого достаточно 
            //для будущей декомпрессии. Пример: abcdefghijklm#. _!?

            foreach (var value in distinctedInputString) //тут заполняется динамически заполняемый словарь и 
            //словарь, сделанный stringBuilder'ом
            {
                var currentStringValue = value.ToString();
                dictionary.Append(currentStringValue);
                dict[currentStringValue] = dict.Count;
            }

            var key = new StringBuilder(); //нужен для сохранения ключей

            var outputValues = new List<int>(); //удобное хранение для выходных кодов, полученных с помощью алгоритма

            foreach (var inputChar in inputString) //тут сделан сам алгоритм (проход по каждому символу исходного текста)
            {
                var keyToString = key.ToString(); //получаем текущий ключ (пустой или ключ, который был на предыдущем шаге)
                var newKey = keyToString + inputChar; //создаём новый ключ (путём добавления символа на текущей итерации)
                if (dict.ContainsKey(newKey)) //если в динамическом словаре уже содержится новый ключ
                {
                    key.Append(inputChar); //тогда добавляем к ключу новый символ
                }
                else //если нового ключа еще нет
                {
                    outputValues.Add(dict[keyToString]); //добавляем на вывод код по ключу текущего ключа
                    dict[newKey] = dict.Count; //присваиваем номер (равен текущему кол-ву элементов в словаре + 1)
                    key = key
                        .Clear()
                        .Append(inputChar); //очищаем текущий ключ и добавляем текущий обрабатываемый символ
                }
            }

            outputValues.Add(dict[key.ToString()]); //по алгоритму добавляем последний код

            var hufInput = new StringBuilder(); //нужен для удобной передачи данных в алгоритм Хаффмана
            hufInput.Append(outputValues.Count);
            hufInput.Append(" ");
            hufInput.Append(string.Join(" ", outputValues));

            HuffmanCompressor.Compress(compressedFilePath, hufInput //запуск алгоритма сжатия по Хаффману
                .ToString()
                .Select(x => x.ToString())
                .ToList(),
                dictionary.ToString());
        }
    }
}