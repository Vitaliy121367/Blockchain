using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blockchain.Service
{
    public class BlockChain
    {
        private int _difficulty = 1;
        public readonly decimal _targetBlockTime = 10; // Target block time in seconds
        public readonly int _adjustEveryNBlocks = 3; // Number of blocks after which difficulty is adjusted
        public List<Block> Chain { get; set; }
        private readonly HashingService _hashingService;
        private readonly MiningService _miningService;
        private readonly TransactionService _transactionService;
        private readonly WalletService _walletService;

        private BlockChain()
        {
            Chain = new List<Block>();
            _hashingService = new HashingService();
            _miningService = new MiningService();
            _transactionService = new TransactionService();
            _walletService = new WalletService();
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
                Transaction = new List<Transaction>(),
                Author = "Genesis",
                PreviousHash = "0",
                TimeStamp = DateTime.UtcNow,
            };

            await _miningService.AsyncMineBlock(genesisBlock, 1);
            Chain.Add(genesisBlock);
        }

        public async Task AddBlock(List<Transaction> transactions, string author)
        {
            bool error = false;
            foreach (var tx in transactions)
            {
                var isValid = _transactionService.ValidTransaction(tx);
                if (!isValid.isValid)
                {
                    Console.WriteLine($"Error Message: {isValid.errorMessage}");
                    error = true;
                }
            }

            var duplicateIds = transactions.Where(tx => tx != null).GroupBy(tx => tx.Id).Where(g => g.Count() > 1).Select(g => g.Key);

            foreach (var id in duplicateIds)
            {
                Console.WriteLine($"Transaction ID {id} is duplicated in the new block.");
                error = true;
            }

            foreach (var tx in transactions)
            {
                if (tx == null)
                    continue;

                bool alreadyExists = Chain.SelectMany(b => b.Transaction).Any(t => t.Id == tx.Id);

                if (alreadyExists)
                {
                    Console.WriteLine($"Transaction ID {tx.Id} already exists in Blockchain.");
                    error = true;
                }
                var isValid = _transactionService.ValidTransaction(tx);
                var isValidSignature = _walletService.VerifySignature(tx.GetDataToSign(), tx.Signature, tx.PublicKey);
                if (!isValid.isValid)
                {
                    Console.WriteLine($"Transaction ID {tx.Id} not valid");
                    error = true;
                }
                if (!isValidSignature)
                {
                    Console.WriteLine($"Transaction ID {tx.Id} Valid Signature invalid: {tx.From} => {tx.To}");
                    error = true;
                }
            }

            if (error)
            {
                return;
            }

            var transactionCopy = new List<Transaction>(transactions);

            var lastBlock = Chain[Chain.Count - 1];
            var newBlock = new Block
            {
                Index = lastBlock.Index + 1,
                Transaction = transactionCopy,
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

        //public List<Block>? SearchBlocksByData(string text)
        //{
        //    if (string.IsNullOrEmpty(text))
        //    {
        //        return new List<Block>();
        //    }
        //    List<Block> blocks = Chain.FindAll(b => b.Data.Contains(text));
        //    return blocks;
        //}

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

        public void FindTransactionById(string id)
        {
            var block = Chain.FirstOrDefault(b => b.Transaction.Any(t => t.Id == id));
            if (block == null)
            {
                Console.WriteLine($"Transaction not found");
                return;
            }
            var transaction = block.Transaction.FirstOrDefault(t => t.Id == id);
            if (block != null && transaction != null)
            {
                Console.WriteLine($"Block Id: {block.Index}");
                Console.WriteLine(transaction);
                return;
            }
        }
        public List<Transaction>? FindTransactionByUser(string user, string filter="all")
        {
            if (filter=="all")
            {
                List<Transaction> transactions = Chain.SelectMany(b => b.Transaction).Where(t => t.From == user || t.To == user).ToList();
                return transactions;
            }
            else if (filter == "from")
            {
                List<Transaction> transactions = Chain.SelectMany(b => b.Transaction).Where(t => t.From == user).ToList();
                return transactions;
            }
            else if (filter == "to")
            {

                List<Transaction> transactions = Chain.SelectMany(b => b.Transaction).Where(t => t.To == user).ToList();
                return transactions;
            }
            return null;
        }


        public bool AuditBlockchain()
        {
            int BlockCount = Chain.Count;
            int TransactionCount = 0;
            decimal TransactionSum = 0;
            bool isValid = true;

            Dictionary<string, int> userOperations = new Dictionary<string, int>();

            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                if (currentBlock.Index == 0)
                    continue;

                if (currentBlock.Hash != _hashingService.ComputeBlockHash(currentBlock))
                {
                    Console.WriteLine($"Block {currentBlock.Index} Hash Error");
                    isValid = false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    Console.WriteLine($"Block {currentBlock.Index} PreviousHash Error");
                    isValid = false;
                }

                if (!currentBlock.Hash.StartsWith(new string('0', currentBlock.DifficultyAtMining)))
                {
                    Console.WriteLine($"Block {currentBlock.Index} Proof of Work Error");
                    isValid = false;
                }

                foreach (var item in currentBlock.Transaction)
                {
                    if (item == null)
                        continue;

                    TransactionCount++;
                    TransactionSum += item.Amount;

                    var isValidItem = _transactionService.ValidTransaction(item);

                    if (!isValidItem.isValid)
                    {
                        Console.WriteLine(isValidItem.errorMessage);
                        isValid = false;
                    }

                    int alreadyExists = Chain
                        .SelectMany(b => b.Transaction)
                        .Count(t => t != null && t.Id == item.Id);

                    if (alreadyExists > 1)
                    {
                        Console.WriteLine($"Transaction ID {item.Id} already exists in Blockchain.");
                        isValid = false;
                    }

                    string userId = item.From;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (userOperations.ContainsKey(userId))
                            userOperations[userId]++;
                        else
                            userOperations[userId] = 1;
                    }
                }
            }

            var mostActiveUser = userOperations
                .OrderByDescending(x => x.Value)
                .FirstOrDefault();

            Console.WriteLine($"Block Count: {BlockCount},");
            Console.WriteLine($"Transaction Count: {TransactionCount},");
            Console.WriteLine($"Transaction Sum: {TransactionSum}");
            Console.WriteLine($"Most Active User: {mostActiveUser.Key ?? "None"}");
            Console.WriteLine($"User Operations: {mostActiveUser.Value}");
            Console.WriteLine($"IsValid: {isValid}");

            return isValid;
        }

    }
}
