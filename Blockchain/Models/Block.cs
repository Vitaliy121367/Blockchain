using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain.Models
{
    public class Block
    {
        public int Index { get; set; }
        public string Hash { get; set; }
        public string Author { get; set; }
        public List<Transaction> Transaction { get; set; }
        public string PreviousHash { get; set; }
        public DateTime TimeStamp { get; set; }
        public long Nonce { get; set; }
        public decimal MiningDuration { get; set; }
        public int DifficultyAtMining { get; set; }
    }
}
