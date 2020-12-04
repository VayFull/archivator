using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DeCompressor.Algorithms
{
    public static class HuffmanDeCompressor
    {
        public static void DeCompress(string fileToDeCompressPath, string deCompressedFilePath)
        {
            var dictionary = new Dictionary<string, string>(); //Код - символ
            string encodedString;

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

                var fileEnd = binReader.ReadInt32();

                var byteString = new StringBuilder();

                while (binReader.BaseStream.Position < binReader.BaseStream.Length)
                {
                    var convertedByteString = new string(Convert.ToString(binReader.ReadByte(), 2).Reverse().ToArray());

                    while (convertedByteString.Length != 8)
                    {
                        convertedByteString += "0";
                    }


                    byteString.Append(convertedByteString);
                }

                encodedString = byteString.Remove(byteString.Length - fileEnd, fileEnd).ToString();
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
    }
}
