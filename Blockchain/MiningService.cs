using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class MiningService
    {
        private readonly HashingService _hashingService;
        public MiningService()
        {
            _hashingService = new HashingService();
        }
        public void MineBlock(Block block, int difficulty)
        {
            string target = new string('0', difficulty);

            while (true)
            {
                block.Hash = _hashingService.ComputeBlockHash(block);

                if (block.Hash.Substring(0, difficulty) == target)
                {
                    Console.WriteLine($"Found Block {block.Index}: Nonce: {block.Nonce}, Hash: {block.Hash}");
                    break;
                }

                if (block.Nonce % 100000 == 0)
                {
                    Console.WriteLine($"Mining Block {block.Index}: Nonce: {block.Nonce}, Hash: {block.Hash}");
                }

                block.Nonce++;
            }
        }
    }
}
