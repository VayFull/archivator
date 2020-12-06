namespace Compressor.Algorithms
{
    public class Block // Класс представляет собой узел дерева кодирования Хаффмана
    {
        public string Symbol { get; set; } // Символ, который хранят только узлы нижнего уровня

        public int Frequency { get; set; } // Вес узла в дереве

        public Block Block1 { get; set; } // Ссылка на дочерний блок

        public Block Block2 { get; set; } // Ссылка на дочерний блок

        public bool Blocked { get; set; } // Отметка, что блок был использован в алгоритме

        public Block(Block block1, Block block2)
        {
            Frequency = block1.Frequency + block2.Frequency;
            Block1 = block1;
            Block2 = block2;
        }

        public Block(string symbol, int frequency)
        {
            Symbol = symbol;
            Frequency = frequency;
        }
    }
}
