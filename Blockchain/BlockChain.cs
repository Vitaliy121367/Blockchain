using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class BlockChain
    {
        public List<Block> Chain { get; set; }
        private readonly HashingService _hashingService;
        private readonly MiningService _miningService;
        public int Difficulty { get; private set; } = 4; // Number of leading zeros required in the hash
        public BlockChain(int difficulty)
        {
            Chain = new List<Block>();
            _hashingService = new HashingService();
            _miningService = new MiningService();
            Difficulty = difficulty;
            AddGenesisBlock();
        }

        private void AddGenesisBlock()
        {
            var genesisBlock = new Block
            {
                Index = 0,
                Data = "GenesisBlock",
                Author = "Genesis",
                PreviousHash = "0",
                TimeStamp = DateTime.UtcNow
            };
            _miningService.MineBlock(genesisBlock,1);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(string data, string author)
        {
            var lastBlock = Chain[Chain.Count - 1];
            var newBlock = new Block
            {
                Index = lastBlock.Index + 1,
                Data = data,
                Author = author,
                PreviousHash = lastBlock.Hash,
                TimeStamp = DateTime.UtcNow
            };
            _miningService.MineBlock(newBlock, Difficulty);
            Chain.Add(newBlock);
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if (currentBlock.Index == 0)
                {
                    continue; // Skip genesis block
                }
                if (currentBlock.Hash != _hashingService.ComputeBlockHash(currentBlock))
                {
                    return false;
                }
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
                if(!currentBlock.Hash.StartsWith(new string('0', Difficulty)))
                {
                    return false;
                }
            }
            return true;
        }

        public Block? SearchBlockByIndex(int index)
        {
            return Chain.Find(b => b.Index == index);
        }

        public List<Block>? SearchBlocksByData(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<Block>();
            }
            List<Block> blocks = Chain.FindAll(b => b.Data.Contains(text));
            return blocks;
        }

        public void RepairChain(int startIndex)
        {
            if(startIndex >= 0 && startIndex < Chain.Count)
            {
                int sumTry = 0;
                DateTime timeStart = DateTime.Now;
                for (int i = startIndex; i < Chain.Count; i++)
                {
                    var currentBlock = Chain[i];
                    if (i > 0)
                    {
                        currentBlock.PreviousHash = Chain[i - 1].Hash;
                    }
                    sumTry += _miningService.MineBlock(currentBlock, Difficulty);
                }
                DateTime timeEnd = DateTime.Now;
                Console.WriteLine($"Chain repaired: {Chain.Count - startIndex}");
                Console.WriteLine($"Sum Try: {sumTry}");
                Console.WriteLine($"Time: {timeEnd - timeStart}");
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}
