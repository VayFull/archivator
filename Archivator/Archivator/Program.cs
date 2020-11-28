using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Archivator
{
    class Coding
    {
        public int Number { get; set; }
        public int Code { get; set; }

        public Coding(int code, int number)
        {
            Code = code;
            Number = number;
        }
    }
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
                    output += dict[s];
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
            int prevcode, currcode;
            string resultDecode = "";

            prevcode = decodeDict
                .First(x => x.Value == int.Parse(output[0].ToString())).Value;
            resultDecode += decodeDict
                .First(x => x.Value == int.Parse(output[0].ToString())).Key;
            output = output.Substring(1, output.Length - 1);
            
            foreach (var outputChar in output)
            {
                currcode = int.Parse(outputChar.ToString());
                entry = decodeDict.First(x => x.Value == currcode).Key;
                resultDecode += entry;
                decodeChar = entry[0];
                var buf = decodeDict.First(x => x.Value == prevcode).Key;
                decodeDict[buf + decodeChar] = decodeDict.Count;
                prevcode = currcode;
            }
            
            Console.WriteLine($"decode result: {resultDecode}");
        }
    }
}