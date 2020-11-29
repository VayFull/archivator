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
        static Dictionary<string, int> dict = new Dictionary<string, int>();
        static Dictionary<int, string> decodeDict = new Dictionary<int, string>();
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
            //var dict = new Dictionary<string, int>();
            //var decodeDict = new Dictionary<int, string>();

            var inputString = File
                .OpenText(fileToCompressPath)
                .ReadToEnd();


            foreach (var value in inputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dict[currentStringValue] = dict.Count;
                    decodeDict[decodeDict.Count] = currentStringValue;
                }
            }

            string s = "";
            char buffer;
            StringBuilder output = new StringBuilder();

            foreach (var inputChar in inputString)
            {
                buffer = inputChar;
                if (dict.ContainsKey(s + buffer))
                {
                    s = s + buffer;
                }
                else
                {
                    output.Append(dict[s] + "\n");
                    dict[s + buffer] = dict.Count;
                    s = buffer.ToString();
                }
            }

            output.Append(dict[s]);

            File.WriteAllText(compressedFilePath, output.ToString());
        }

        public static void DeCompress(string fileToDeCompressPath, string deCompressedFilePath)
        {
            StringBuilder decodeResult = new StringBuilder();

            var decodeCodes = File.ReadAllLines(fileToDeCompressPath)
                .Select(x => int.Parse(x))
                .ToList();

            var previousCode = decodeCodes[0];
            var firstPhrase = decodeDict[previousCode];

            decodeResult.Append(firstPhrase);

            for (int i = 1; i < decodeCodes.Count; i++)
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