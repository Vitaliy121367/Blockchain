using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Blockchain.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] Signature { get; set; }
        public bool isCoinBase { get; set; }

        public Transaction(string from, string to, decimal amount, byte[] publicKey)
        {
            Id = Guid.NewGuid().ToString();
            From = from;
            To = to;
            Amount = amount;
            PublicKey = publicKey;
            Timestamp = DateTime.UtcNow;
            isCoinBase = false;
        }

        public static Transaction CreateCoinBaseTransaction(string minerAddress, decimal amount)
        {
            var tx = new Transaction("COINBASE", minerAddress, amount, null);
            tx.isCoinBase = true;
            return tx;
        }

        public string ToRawString()
        {
            return $"{Id}|{From}|{To}|{Amount}|{Timestamp}";
        }

        public override string ToString()
        {
            return $"Transaction Id: {Id}, From: {From}, To: {To}, Amount: {Amount}, Timestamp: {Timestamp}";
        }

        public byte[] GetDataToSign()
        {
            string dataToSign = $"{From}|{To}|{Amount}|{Timestamp}";
            return Encoding.UTF8.GetBytes(dataToSign);
        }

    }
}
