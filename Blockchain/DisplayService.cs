using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
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
                Console.WriteLine($"Data: {block.Data}");
                Console.WriteLine($"Previous Hash: {block.PreviousHash}");
                Console.WriteLine($"Timestamp: {block.TimeStamp}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine(new string('-', 50));
            }
        }
        public void DisplyBlock(Block block)
        {
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Author: {block.Author}");
            Console.WriteLine($"Data: {block.Data}");
            Console.WriteLine($"Previous Hash: {block.PreviousHash}");
            Console.WriteLine($"Timestamp: {block.TimeStamp}");
            Console.WriteLine(new string('-', 50));
        }
    }
}
