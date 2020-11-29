using System;
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
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var dict = new Dictionary<string, int>();
            var decodeDict = new Dictionary<int, string>();
            string result = "";
            /*Console.WriteLine("Введите input");
            var input = Console.ReadLine();
            
            Console.WriteLine("Введите output");
            var output = Console.ReadLine();*/

            var location = Assembly
                .GetExecutingAssembly()
                .Location
                .Replace("Archivator.dll", "");
            
            var readLocation = location + "file.txt";

            var file = File
                .OpenText(readLocation);
            
            var inputString = file
                .ReadToEnd();

            var orderedInputString = inputString
                .OrderBy(x => x);

            foreach (var value in orderedInputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dict[currentStringValue] = dict.Count;
                    decodeDict[decodeDict.Count] = currentStringValue;
                }
            }

            string s = "";
            char ch;
            StringBuilder output = new StringBuilder();

            foreach (var inputChar in inputString)
            {
                ch = inputChar;
                if (dict.ContainsKey(s + ch))
                {
                    s = s + ch;
                }
                else
                {
                    output.Append(dict[s] + "\n");
                    dict[s + ch] = dict.Count;
                    s = ch.ToString();
                }
            }

            output.Append(dict[s]);

            var writeLocation = location += "archived.txt";
            File.WriteAllText(writeLocation, output.ToString());
            
            stopWatch.Stop();
            var compressedTime = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(compressedTime);
            
            stopWatch.Start();
            
            StringBuilder decodeResult = new StringBuilder();

            var decodeCodes = File.ReadAllLines(location)
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
            
            stopWatch.Stop();
            var decompressedTime = stopWatch.ElapsedMilliseconds;
            Console.WriteLine(decompressedTime);
            //Console.WriteLine($"decode result: {resultDecode}");
        }
    }
}