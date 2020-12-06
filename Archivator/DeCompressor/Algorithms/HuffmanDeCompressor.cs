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
            var dictionary = new Dictionary<string, char>();
            string encodedString = string.Empty;
            string lzwDict;

            using (FileStream fs = new FileStream(fileToDeCompressPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader binReader = new BinaryReader(fs))
            {
                lzwDict = binReader.ReadString();
                int quantity = binReader.ReadInt32();

                for (int i = 0; i < quantity; i++)
                {
                    var code = binReader.ReadString();
                    var symbol = binReader.ReadChar();
                    dictionary.Add(code, symbol);
                }

                var trashBitsCount = binReader.ReadInt32();

                var byteString = new StringBuilder();

                var bytesArray = binReader.ReadBytes((int)(binReader.BaseStream.Length - binReader.BaseStream.Position));

                foreach (var oneByte in bytesArray)
                {
                    var convertedByteString = new StringBuilder(new string(Convert.ToString(oneByte, 2).Reverse().ToArray()));

                    var convertedShift = 8 - convertedByteString.Length;

                    convertedByteString.Append('0', convertedShift);

                    byteString.Append(convertedByteString);
                }

                if (byteString.Length != 0)
                    encodedString = byteString.Remove(byteString.Length - trashBitsCount, trashBitsCount).ToString();
            }

            var decodeResult = new StringBuilder();
            var buffer = new StringBuilder();

            foreach (var item in encodedString)
            {
                buffer.Append(item);

                var bufferToString = buffer.ToString();

                if (dictionary.ContainsKey(bufferToString))
                {
                    decodeResult.Append(dictionary[bufferToString]);
                    buffer.Clear();
                }
            }

            LZWDeCompressor.DeCompress(deCompressedFilePath, decodeResult.ToString(), lzwDict);
        }
    }
}
