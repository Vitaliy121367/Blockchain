using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class BlockChain
    {
        public List<Block> Chain { get; set; }
        private readonly HashingService _hashingService;
        public BlockChain()
        {
            Chain = new List<Block>();
            _hashingService = new HashingService();
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
                TimeStamp = DateTime.Parse("2023-06-01")
            };
            genesisBlock.Hash = _hashingService.ComputeBlockHash(genesisBlock);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(string author, string data)
        {
            var lastBlock = Chain[Chain.Count - 1];
            var newBlock = new Block
            {
                Index = lastBlock.Index + 1,
                Author = author,
                Data = data,
                PreviousHash = lastBlock.Hash,
                TimeStamp = DateTime.Now
            };
            newBlock.Hash = _hashingService.ComputeBlockHash(newBlock);
            Chain.Add(newBlock);
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if (currentBlock.Hash != _hashingService.ComputeBlockHash(currentBlock))
                {
                    return false;
                }
                if (currentBlock.PreviousHash != previousBlock.Hash)
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
    }
}
