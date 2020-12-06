using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compressor.Algorithms
{
    public static class LZWCompressor
    {
        public static void Compress(string fileToCompressPath, string compressedFilePath)
        {
            var dict = new Dictionary<string, int>();
            var inputString = File
                .OpenText(fileToCompressPath)
                .ReadToEnd();

            var distinctedInputString = inputString.Distinct();

            StringBuilder dictionary = new StringBuilder();

            foreach (var value in distinctedInputString)
            {
                var currentStringValue = value.ToString();
                if (!dict.ContainsKey(currentStringValue))
                {
                    dictionary.Append(currentStringValue);
                    dict[currentStringValue] = dict.Count;
                }
            }

            var key = new StringBuilder();

            var outputValues = new List<int>();

            foreach (var inputChar in inputString)
            {
                var keyToString = key.ToString();
                var newKey = keyToString + inputChar;
                if (dict.ContainsKey(newKey))
                {
                    key.Append(inputChar);
                }
                else
                {
                    outputValues.Add(dict[keyToString]);
                    dict[newKey] = dict.Count;
                    key = key
                        .Clear()
                        .Append(inputChar);
                }
            }

            outputValues.Add(dict[key.ToString()]);

            var hufInput = new StringBuilder();
            hufInput.Append(outputValues.Count);
            hufInput.Append(" ");
            hufInput.Append(string.Join(" ", outputValues));

            HuffmanCompressor.Compress(compressedFilePath, hufInput
                .ToString()
                .Select(x => x.ToString())
                .ToList(),
                    dictionary
                    .ToString());
        }
    }
}