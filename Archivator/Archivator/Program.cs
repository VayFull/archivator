using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archivator
{
    class Program
    {
        static void Main(string[] args)
        {
            var location = Assembly
                .GetExecutingAssembly()
                .Location
                .Replace("Archivator.dll", "");

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Compress(location + "file.txt", location + "archived.txt");
            stopWatch.Stop();
            System.Console.WriteLine(stopWatch.ElapsedMilliseconds);

            stopWatch.Reset();
            stopWatch.Start();
            DeCompress(location + "archived.txt", location + "deCompressed.txt");
            stopWatch.Stop();
            System.Console.WriteLine(stopWatch.ElapsedMilliseconds);
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