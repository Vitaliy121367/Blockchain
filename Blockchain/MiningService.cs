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
        public int MineBlock(Block block, int difficulty)
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
            return block.Nonce;
        }

        public Task<int> AsyncMineBlock(Block block, int difficulty)
        {
            return Task.Run(() =>
            {
                string target = new string('0', difficulty);

                int foundNonce = -1;
                string foundHash = null;

                object lockObject = new object();

                Parallel.For(0, int.MaxValue, (nonce, state) =>
                {
                    var candidate = new Block
                    {
                        Index = block.Index,
                        PreviousHash = block.PreviousHash,
                        TimeStamp = block.TimeStamp,
                        Author = block.Author,
                        Data = block.Data,
                        Nonce = nonce
                    };

                    string hash = _hashingService.ComputeBlockHash(candidate);

                    if (hash.StartsWith(target))
                    {
                        lock (lockObject)
                        {
                            if (foundNonce == -1)
                            {
                                foundNonce = nonce;
                                foundHash = hash;
                            }
                        }

                        state.Stop();
                    }
                });

                if (foundNonce == -1)
                    throw new Exception("Mining failed");

                block.Nonce = foundNonce;
                block.Hash = foundHash;

                Console.WriteLine(
                    $"Found Block {block.Index}: Nonce: {block.Nonce}, Hash: {block.Hash}");

                return foundNonce;
            });
        }
    }
}
