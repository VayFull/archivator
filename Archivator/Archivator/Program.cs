using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Archivator
{
    class Program
    {
        static void Main(string[] args)
        {
            var dict = new Dictionary<string, int>();
            var decodeDict = new Dictionary<string, int>();
            string result = "";
            /*Console.WriteLine("Введите input");
            var input = Console.ReadLine();
            
            Console.WriteLine("Введите output");
            var output = Console.ReadLine();*/

            var location = Assembly.GetExecutingAssembly().Location.Replace("Archivator.dll", "");
            var readLocation = location + "file.txt";

            var file = File.OpenText(readLocation);
            var inputString = file.ReadToEnd();

            var orderedInputString = inputString.OrderBy(x => x);

            foreach (var value in orderedInputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dict[currentStringValue] = dict.Count;
                    decodeDict[currentStringValue] = decodeDict.Count;
                }
            }
            
            string s = "";
            char ch;
            string output = "";

            foreach (var inputChar in inputString)
            {
                ch = inputChar;
                if (dict.ContainsKey(s + ch))
                {
                    s = s + ch;
                }
                else
                {
                    output += dict[s] + "\n";
                    dict[s + ch] = dict.Count;
                    s = ch.ToString();
                }
            }

            output += dict[s];

            foreach (var keyValue in dict)
            {
                Console.WriteLine($"key: {keyValue.Key} code: {keyValue.Value}\n");
            }

            var writeLocation = location += "archived.txt";
            File.WriteAllText(writeLocation, output);
            Console.WriteLine($"Result: = {result}");

            string entry;
            char decodeChar;
            int prevcode = 0, currcode;
            string resultDecode = "";

            var decodeCodes = File.ReadAllLines(location)
                .Select(x => int.Parse(x))
                .ToList();

            var previousCode = decodeCodes[0];
            var firstPhrase = decodeDict
                .First(x => x.Value == previousCode)
                .Key;

            resultDecode += firstPhrase;

            for (int i = 1; i < decodeCodes.Count; i++)
            {
                var currentCode = decodeCodes[i];

                if (decodeDict.ContainsValue(currentCode))
                {
                    var currentPhrase = decodeDict
                        .First(x => x.Value == currentCode)
                        .Key;
                    resultDecode += currentPhrase;

                    var previousPhrase = decodeDict
                        .First(x => x.Value == previousCode)
                        .Key;

                    decodeDict[previousPhrase + currentPhrase[0]] = decodeDict.Count;

                    previousCode = currentCode;
                }
                else
                {
                    var currentPhrase = decodeDict
                        .First(x => x.Value == previousCode)
                        .Key;

                    currentPhrase += currentPhrase[0];

                    decodeDict[currentPhrase] = decodeDict.Count;

                    resultDecode += currentPhrase;

                    previousCode = currentCode;
                }
               
            }

            Console.WriteLine($"decode result: {resultDecode}");
        }

        public static int GetCurrentCode(Dictionary<string, int> dict, string output, int index, out int shift)
        {
            shift = 0;
            var prevCode = int.Parse(output[index].ToString());
            var dictionaryContainsMoreCode = true;
            if (prevCode == 0) return prevCode;
            
            
            while (dictionaryContainsMoreCode)
            {
                if (output.Length == index + 1)
                {
                    return prevCode;
                }
                var nextCodeValue = int.Parse(prevCode.ToString() + output[index + 1].ToString());
                if (dict.ContainsValue(nextCodeValue))
                {
                    prevCode = nextCodeValue;
                    index++;
                    shift++;
                }
                else
                {
                    dictionaryContainsMoreCode = false;
                }
            }

            return prevCode;
        }
    }
}