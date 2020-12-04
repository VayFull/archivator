namespace Compressor.Algorithms
{
    public class Block
    {
        public string Symbol { get; set; }

        public int Frequency { get; set; }

        public Block Block1 { get; set; }

        public Block Block2 { get; set; }

        public bool Blocked { get; set; }

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
