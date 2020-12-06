using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeCompressor.Algorithms
{
    public static class LZWDeCompressor
    {
        public static void DeCompress(string deCompressedFilePath, string huffmanResult, string lzwDict)
        {
            var decodeDict = new Dictionary<int, string>();
            StringBuilder decodeResult = new StringBuilder();
            int[] decodeCodes;


            var encodedResult = huffmanResult.Split(' ');

            foreach (var value in lzwDict)
                decodeDict[decodeDict.Count] = value.ToString();

            int quantity = int.Parse(encodedResult[0]);

            decodeCodes = new int[quantity];

            for (int i = 1; i < encodedResult.Length; i++)
                decodeCodes[i - 1] = int.Parse(encodedResult[i]);

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