﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compressor.Algorithms
{
    public static class HuffmanCompressor
    {
        public static void Compress(string compressedFilePath, List<string> inputString, string lzwDict)
        {
            var dictionaryOfEntries = new Dictionary<string, int>();
            var tableBlocks = new List<Block>();

            var inputLength = inputString.Count();

            foreach (var value in inputString)
            {
                if (!dictionaryOfEntries.ContainsKey(value))
                    dictionaryOfEntries.Add(value, 1);
                else
                    dictionaryOfEntries[value] += 1;
            }

            foreach (var item in dictionaryOfEntries)
                tableBlocks.Add(new Block(item.Key, item.Value));

            tableBlocks = tableBlocks
                .OrderByDescending(x => x.Frequency)
                .ToList();

            if (tableBlocks.Count > 1)
                while (true)
                {
                    var block1 = tableBlocks
                        .Last(x => !x.Blocked);
                    var block2 = tableBlocks
                        .Last(x => !x.Blocked && x != block1);

                    tableBlocks.Add(new Block(block1, block2));

                    block1.Blocked = true;
                    block2.Blocked = true;

                    tableBlocks = tableBlocks
                        .OrderByDescending(x => x.Frequency)
                        .ToList();

                    if (block1.Frequency + block2.Frequency >= inputLength)
                        break;
                }

            var dictionary = new Dictionary<string, string>(); //Символ - код

            if (tableBlocks.Count != 0)
                if (tableBlocks.Count == 1)
                    dictionary.Add(tableBlocks.First().Symbol, "0");
                else
                    GoDown(tableBlocks.First(), new StringBuilder());

            void GoDown(Block block, StringBuilder currentValue)
            {
                var block1 = block.Block1;
                var block2 = block.Block2;

                var currentValueToString = currentValue.ToString();

                if (block1.Symbol != null)
                {
                    dictionary.Add(block1.Symbol, currentValueToString + "0");
                }
                else
                {
                    currentValue.Append("0");
                    GoDown(block1, currentValue);
                }

                if (block2.Symbol != null)
                {
                    dictionary.Add(block2.Symbol, currentValueToString + "1");
                }
                else
                {
                    currentValue.Append("1");
                    GoDown(block2, currentValue);
                }

                if (currentValue.Length > 0)
                    currentValue.Remove(currentValue.Length - 1, 1);
            }

            var compressedString = string.Join("", inputString.Select(x => dictionary[x]));

            using (FileStream fs = new FileStream(compressedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter binWriter = new BinaryWriter(fs))
            {
                var dictionaryLength = dictionary.Count;

                binWriter.Write(lzwDict); //lzw
                binWriter.Write(dictionaryLength);

                foreach (var pair in dictionary)
                {
                    binWriter.Write(pair.Value);
                    binWriter.Write(pair.Key[0]);
                }

                int trashBitsCount = 0;
                var shift = compressedString.Length % 8;
                if (shift != 0)
                    trashBitsCount = 8 - shift;

                var compressedStringBuilder = new StringBuilder(compressedString);

                for (int i = 0; i < trashBitsCount; i++)
                {
                    compressedStringBuilder.Append("0");
                }

                var compressedBits = compressedString
                    .Select(x => x == '1');

                BitArray bitArray = new BitArray(compressedBits.ToArray());

                var bytes = new byte[bitArray.Length / 8 + (bitArray.Length % 8 == 0 ? 0 : 1)];
                bitArray.CopyTo(bytes, 0);

                binWriter.Write(trashBitsCount);
                binWriter.Write(bytes);
            }
        }
    }
}
