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

            //using (FileStream fs = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            //using (BinaryWriter binWriter = new BinaryWriter(fs))
            //{
            //    binWriter.Write(dictionary.ToString());
            //    binWriter.Write(outputValues.Count);
            //    foreach (var compressedValue in outputValues)
            //        binWriter.Write(compressedValue);
            //}

            var hufInput = new StringBuilder();
            hufInput.Append(outputValues.Count);
            hufInput.Append(" ");
            hufInput.Append(string.Join(" ", outputValues));

            HuffmanCompressor.Compress(compressedFilePath, hufInput.ToString().Select(x => x.ToString()).ToList(), dictionary.ToString());
        }
    }
}