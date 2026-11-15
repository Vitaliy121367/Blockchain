using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Blockchain.Service
{
    public class MiningService
    {
        private readonly HashingService _hashingService;
        public MiningService()
        {
            _hashingService = new HashingService();
        }
        public long MineBlock(Block block, int difficulty)
        {
            string target = new string('0', difficulty);
            var startTime = Stopwatch.StartNew();

            while (true)
            {
                block.Hash = _hashingService.ComputeBlockHash(block);

                if (block.Hash.Substring(0, difficulty) == target)
                {
                    startTime.Stop();
                    block.MiningDuration = (decimal)startTime.Elapsed.TotalSeconds;
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


        public Task<long> AsyncMineBlock(Block block, int difficulty)
        {
            return Task.Run(() =>
            {
                string target = new string('0', difficulty);
                var startTime = Stopwatch.StartNew();

                long foundNonce = -1;
                string foundHash = null;

                object lockObject = new object();

                Parallel.For(0, int.MaxValue, (nonce, state) =>
                {
                    if (state.IsStopped)
                        return;

                    var candidate = new Block
                    {
                        Index = block.Index,
                        PreviousHash = block.PreviousHash,
                        TimeStamp = block.TimeStamp,
                        Author = block.Author,
                        Transaction = block.Transaction,
                        Nonce = nonce,
                        DifficultyAtMining = block.DifficultyAtMining
                    };

                    string hash = _hashingService.ComputeBlockHash(candidate);

                    if (hash.StartsWith(target, StringComparison.Ordinal))
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

                startTime.Stop();

                block.MiningDuration =
                    (decimal)startTime.Elapsed.TotalSeconds;

                block.Nonce = foundNonce;
                block.Hash = foundHash;

                if (block.Nonce % 100000 == 0)
                {
                    Console.WriteLine($"Mining Block {block.Index}: Nonce: {block.Nonce}, Hash: {block.Hash}");
                }

                Console.WriteLine($"Found Block {block.Index}: Nonce: {block.Nonce}, Hash: {block.Hash}");

                return foundNonce;
            });
        }

    }
}
