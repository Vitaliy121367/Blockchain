using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class Block
    {
        public int Index { get; set; }
        public string Hash { get; set; }
        public string Author { get; set; }
        public string Data { get; set; }
        public string PreviousHash { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
