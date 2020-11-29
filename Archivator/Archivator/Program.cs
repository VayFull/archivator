using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archivator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var location = Assembly
                .GetExecutingAssembly()
                .Location
                .Replace("Archivator.dll", "");

            var stopWatch = new Stopwatch();

            stopWatch.Start();
            HCompress(location + "file.txt", location + "archived.q");
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();

            stopWatch.Start();
            HDeCompress(location + "archived.q", location + "deCompressed.txt");
            stopWatch.Stop();
            Console.WriteLine(stopWatch.ElapsedMilliseconds);


            //stopWatch.Start();
            //Compress(location + "file.txt", location + "archived.txt");
            //stopWatch.Stop();
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds);

            //stopWatch.Reset();
            //stopWatch.Start();
            //DeCompress(location + "archived.txt", location + "deCompressed.txt");
            //stopWatch.Stop();
            //System.Console.WriteLine(stopWatch.ElapsedMilliseconds);
        }

        public class Block
        {
            public string Symbol { get; set; }

            public int Frequency { get; set; }

            public Block Block1 { get; set; }

            public Block Block2 { get; set; }

            public bool Blocked { get; set; }

            public Block(Block block1, Block block2)
            {
                Frequency = block1.Frequency + block2.Frequency;
                Block1 = block1;
                Block2 = block2;
            }

            public Block(string symbol, int frequency)
            {
                Symbol = symbol;
                Frequency = frequency;
            }
        }

        public static void HCompress(string fileToCompressPath, string compressedFilePath)
        {
            var inputString = File
                .OpenText(fileToCompressPath)
                .ReadToEnd()
                .Select(x => x.ToString());

            var dictionaryOfEntries = new Dictionary<string, int>();
            var tableBlocks = new List<Block>();

            var inputLength = inputString.Count();

            foreach (var value in inputString)
            {
                if (!dictionaryOfEntries.ContainsKey(value))
                    dictionaryOfEntries.Add(value, 1);
                else
                    dictionaryOfEntries[value] += 1;
            }

            foreach (var item in dictionaryOfEntries)
                tableBlocks.Add(new Block(item.Key, item.Value));

            tableBlocks = tableBlocks
                .OrderByDescending(x => x.Frequency)
                .ToList();

            while (true)
            {
                var block1 = tableBlocks
                    .Last(x => !x.Blocked);
                var block2 = tableBlocks
                    .Last(x => !x.Blocked && x != block1);

                tableBlocks.Add(new Block(block1, block2));

                block1.Blocked = true;
                block2.Blocked = true;

                tableBlocks = tableBlocks
                    .OrderByDescending(x => x.Frequency)
                    .ToList();

                if (block1.Frequency + block2.Frequency >= inputLength)
                    break;
            }

            var dictionary = new Dictionary<string, string>(); ///Символ - код

            GoDown(tableBlocks.First(), new StringBuilder());

            void GoDown(Block block, StringBuilder currentValue)
            {
                var block1 = block.Block1;
                var block2 = block.Block2;

                if (block1.Symbol != null)
                {
                    dictionary.Add(block1.Symbol, currentValue.ToString() + "0");
                }
                else
                {
                    currentValue.Append("0");
                    GoDown(block1, currentValue);
                }

                if (block2.Symbol != null)
                {
                    dictionary.Add(block2.Symbol, currentValue.ToString() + "1");
                }
                else
                {
                    currentValue.Append("1");
                    GoDown(block2, currentValue);
                }

                if (currentValue.Length > 0)
                    currentValue.Remove(currentValue.Length - 1, 1);
            }

            var compressedString = string.Join("" ,inputString.Select(x => dictionary[x]));

            using (FileStream fs = new FileStream(compressedFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            using (BinaryWriter binWriter = new BinaryWriter(fs))
            {
                var dictionaryLength = dictionary.Count;

                binWriter.Write(dictionaryLength);
                foreach (var pair in dictionary)
                {
                    binWriter.Write(Convert.ToInt32(pair.Value, 2));
                    binWriter.Write(pair.Key[0]);
                }
                binWriter.Write(compressedString);
            }
        }

        public static void HDeCompress(string fileToDeCompressPath, string deCompressedFilePath)
        {
            var dictionary = new Dictionary<string, string>();/// Код - символ
            string encodedString = "";

            using (FileStream fs = new FileStream(fileToDeCompressPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader binReader = new BinaryReader(fs))
            {
                int quantity = binReader.ReadInt32();

                for (int i = 0; i < quantity; i++)
                {
                    var code = binReader.ReadString();
                    var symbol = binReader.ReadChar();
                    dictionary.Add(code, symbol.ToString());
                }
                encodedString = binReader.ReadString();
            }

            var decodeResult = new StringBuilder();
            var buffer = new StringBuilder();

            foreach (var item in encodedString)
            {
                buffer.Append(item);

                if (dictionary.ContainsKey(buffer.ToString()))
                {
                    decodeResult.Append(dictionary[buffer.ToString()]);
                    buffer.Clear();
                }
            }

            File.WriteAllText(deCompressedFilePath, decodeResult.ToString());
        }

        public static void Compress(string fileToCompressPath, string compressedFilePath)
        {
            var dict = new Dictionary<string, int>();

            var inputString = File
                .OpenText(fileToCompressPath)
                .ReadToEnd();

            StringBuilder dictionary = new StringBuilder();

            foreach (var value in inputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dictionary.Append(currentStringValue);
                    dict[currentStringValue] = dict.Count;
                }
            }

            var key = new StringBuilder();
            char buffer;

            var outputValues = new List<int>();

            foreach (var inputChar in inputString)
            {
                buffer = inputChar;
                if (dict.ContainsKey(key.ToString() + buffer))
                {
                    key.Append(buffer);
                }
                else
                {
                    outputValues.Add(dict[key.ToString()]);
                    dict[key.ToString() + buffer] = dict.Count;
                    key = new StringBuilder(buffer.ToString());
                }
            }

            outputValues.Add(dict[key.ToString()]);

            using (FileStream fs = new FileStream(compressedFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            using (BinaryWriter binWriter = new BinaryWriter(fs))
            {
                binWriter.Write(dictionary.ToString());
                binWriter.Write(outputValues.Count);
                foreach (var compressedValue in outputValues)
                    binWriter.Write(compressedValue);
            }
        }

        public static void DeCompress(string fileToDeCompressPath, string deCompressedFilePath)
        {
            var decodeDict = new Dictionary<int, string>();
            StringBuilder decodeResult = new StringBuilder();
            int[] decodeCodes;

            using (FileStream fs = new FileStream(fileToDeCompressPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader binReader = new BinaryReader(fs))
            {
                foreach (var value in binReader.ReadString())
                    decodeDict[decodeDict.Count] = value.ToString();

                int quantity = binReader.ReadInt32();

                decodeCodes = new int[quantity];

                for (int i = 0; i < quantity; i++)
                    decodeCodes[i] = binReader.ReadInt32();
            }

            var previousCode = decodeCodes[0];
            var firstPhrase = decodeDict[previousCode];

            decodeResult.Append(firstPhrase);

            for (int i = 1; i < decodeCodes.Length; i++)
            {
                var currentCode = decodeCodes[i];

                if (decodeDict.ContainsKey(currentCode))
                {
                    var currentPhrase = decodeDict[currentCode];
                    decodeResult.Append(currentPhrase);

                    var previousPhrase = decodeDict[previousCode];
                    decodeDict[decodeDict.Count] = previousPhrase + currentPhrase[0];

                    previousCode = currentCode;
                }
                else
                {
                    var currentPhrase = decodeDict[previousCode];

                    currentPhrase += currentPhrase[0];

                    decodeDict[decodeDict.Count] = currentPhrase;

                    decodeResult.Append(currentPhrase);

                    previousCode = currentCode;
                }
            }

            File.WriteAllText(deCompressedFilePath, decodeResult.ToString());
        }
    }
}