using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class BlockChain
    {
        private int _difficulty = 1;
        public readonly decimal _targetBlockTime = 10; // Target block time in seconds
        public readonly int _adjustEveryNBlocks = 3; // Number of blocks after which difficulty is adjusted
        public List<Block> Chain { get; set; }
        private readonly HashingService _hashingService;
        private readonly MiningService _miningService;
        private BlockChain()
        {
            Chain = new List<Block>();
            _hashingService = new HashingService();
            _miningService = new MiningService();
        }

        public static async Task<BlockChain> CreateAsync()
        {
            var blockchain = new BlockChain();

            await blockchain.AddGenesisBlock();

            return blockchain;
        }

        private async Task AddGenesisBlock()
        {
            var genesisBlock = new Block
            {
                Index = 0,
                Data = "GenesisBlock",
                Author = "Genesis",
                PreviousHash = "0",
                TimeStamp = DateTime.UtcNow,
                DifficultyAtMining = _difficulty
            };

            await _miningService.AsyncMineBlock(genesisBlock, 1);
            Chain.Add(genesisBlock);
        }

        public async Task AddBlock(string data, string author)
        {
            var lastBlock = Chain[Chain.Count - 1];
            var newBlock = new Block
            {
                Index = lastBlock.Index + 1,
                Data = data,
                Author = author,
                PreviousHash = lastBlock.Hash,
                TimeStamp = DateTime.UtcNow,
                DifficultyAtMining = _difficulty
            };
            await _miningService.AsyncMineBlock(newBlock, _difficulty);
            Chain.Add(newBlock);
            AdjustDifficulty();
        }
        private void AdjustDifficulty()
        {
            var rexentBlock = Chain.Where(b => b.Index > 0).TakeLast(_adjustEveryNBlocks).ToList();
            if (rexentBlock.Count < _adjustEveryNBlocks)
            {
                return; // Not enough blocks to adjust difficulty
            }
            var avgTime = rexentBlock.Average(b => (b.MiningDuration));
            if (avgTime < _targetBlockTime)
            {
                _difficulty++;
                Console.WriteLine($"Difficulty increased to {_difficulty}");
            }
            else if (avgTime > _targetBlockTime)
            {
                _difficulty = Math.Max(1, _difficulty - 1);
                Console.WriteLine($"Difficulty decreased to {_difficulty}");
            }
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
                if(!currentBlock.Hash.StartsWith(new string('0', currentBlock.DifficultyAtMining)))
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

        public async Task RepairChain(int startIndex)
        {
            if (startIndex < 0 || startIndex >= Chain.Count)
                return;

            DateTime timeStart = DateTime.Now;

            for (int i = startIndex; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];

                if (i > 0)
                {
                    currentBlock.PreviousHash = Chain[i - 1].Hash;
                }

                await _miningService.AsyncMineBlock(currentBlock, _difficulty);
            }

            DateTime timeEnd = DateTime.Now;

            Console.WriteLine($"Chain repaired: {Chain.Count - startIndex}");
            Console.WriteLine($"Time: {timeEnd - timeStart}");
            Console.WriteLine(new string('-', 50));
        }

        public void PrintDifficultyHistory()
        {
            foreach (var block in Chain)
            {
                Console.WriteLine($"Index: {block.Index}, Difficulty: {block.DifficultyAtMining}, Nonce: {block.Nonce}, MiningDuration: {block.MiningDuration}");
            }
        }

        public string FindFirstInvalidBlock()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if (currentBlock.Index == 0)
                {
                    continue; // Skip genesis block
                }
                if (!currentBlock.Hash.StartsWith(new string('0', currentBlock.DifficultyAtMining)))
                {
                    return $"Block {currentBlock.Index} Proof of Work Error";
                }
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return $"Block {currentBlock.Index} PreviousHash Error";
                }
                if (currentBlock.Hash != _hashingService.ComputeBlockHash(currentBlock))
                {
                    return $"Block {currentBlock.Index} Hash Error";
                }
            }
            return "Chain is valid";
        }
    }
}
