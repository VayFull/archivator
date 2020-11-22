using System;
using System.Collections.Generic;
using System.IO;
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
            var dict = new Dictionary<string, Coding>();
            string result = "";
            /*Console.WriteLine("Введите input");
            var input = Console.ReadLine();
            
            Console.WriteLine("Введите output");
            var output = Console.ReadLine();*/

            var location = Assembly.GetExecutingAssembly().Location.Replace("Archivator.dll", "");
            var readLocation = location + "file.txt";

            var file = File.OpenText(readLocation);
            var inputString = file.ReadToEnd();
            
            foreach (var value in inputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dict[currentStringValue] = new Coding(dict.Count, dict.Count);
                }
            }

            string currentPhrase = "";
            string previousPhrase = "";
            
            for (int index = 0; index < inputString.Length; index++)
            {
                previousPhrase = currentPhrase;
                currentPhrase += inputString[index].ToString();
                
                if (index == inputString.Length - 1)
                {
                    dict[currentPhrase] = new Coding(dict[previousPhrase].Number, dict.Count);
                    result += dict[previousPhrase].Number;
                    result += dict[inputString[index].ToString()].Code;
                }

                if (!dict.ContainsKey(currentPhrase))
                {
                    var code = new Coding(dict[previousPhrase].Number, dict.Count);
                    dict[currentPhrase] = code;
                    currentPhrase = "";
                    previousPhrase = "";
                    index--;
                    result += code.Code;
                }
            }

            foreach (var keyValue in dict)
            {
                Console.WriteLine($"key: {keyValue.Key} code: {keyValue.Value.Code} number: {keyValue.Value.Number}\n");
            }

            var writeLocation = location += "archived.txt";
            File.WriteAllText(writeLocation, result);
            Console.WriteLine($"Result: = {result}");
        }
    }
}