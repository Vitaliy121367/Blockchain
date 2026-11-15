using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Service
{
    public class HashingService
    {
        public string ComputeBlockHash(Block block)
        {
            var stringTransactionRaw = string.Concat(block.Transaction.Select(t => t.ToRawString()));
            var dataToHash = $"{block.Index}{block.PreviousHash}{block.TimeStamp}{block.Author}{stringTransactionRaw}{block.Nonce}{block.DifficultyAtMining}"; 
            return ComputeHash(dataToHash);
        }
        private string ComputeHash(string data)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
