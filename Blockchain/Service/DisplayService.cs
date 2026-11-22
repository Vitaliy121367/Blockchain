using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain.Service
{
    public class DisplayService
    {
        public void DisplayChain(BlockChain blockChain)
        {
            foreach (var block in blockChain.Chain)
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine($"Author: {block.Author}");
                Console.WriteLine($"Transaction:");
                foreach (var item in block.Transaction)
                {
                    Console.WriteLine("\t"+item);
                }
                Console.WriteLine($"Previous Hash: {block.PreviousHash}");
                Console.WriteLine($"Timestamp: {block.TimeStamp}");
                Console.WriteLine($"DifficultyAtMining: {block.DifficultyAtMining}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine(new string('-', 50));
            }
        }
        public void DisplyBlock(Block block)
        {
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Author: {block.Author}");
            Console.WriteLine($"Transaction:");
            foreach (var item in block.Transaction)
            {
                Console.WriteLine("\t" + item);
            }
            Console.WriteLine($"Previous Hash: {block.PreviousHash}");
            Console.WriteLine($"Timestamp: {block.TimeStamp}");
            Console.WriteLine($"DifficultyAtMining: {block.DifficultyAtMining}");
            Console.WriteLine($"MiningDuration: {block.MiningDuration}");
            Console.WriteLine(new string('-', 50));
        }
        public void PrintTransactionHistory(BlockChain blockchain, Wallet wallet)
        {
            foreach (var block in blockchain.Chain)
            {
                foreach (var item in block.Transaction)
                {
                    if (item.From == wallet.Address || item.To == wallet.Address)
                    {
                        Console.WriteLine(item);
                    }
                }
            }
        }
    }
}
