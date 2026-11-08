using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public class HashingService
    {
        public string ComputeBlockHash(Block block)
        {
            var dataToHash = $"{block.Index}{block.PreviousHash}{block.TimeStamp}{block.Author}{block.Data}{block.Nonce}"; 
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
