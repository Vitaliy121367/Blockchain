using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain.Service
{
    public class TransactionService
    {
        public Transaction CreateTransaction(string from, string to, decimal amount)
        {
            var tx = new Transaction(from, to, amount);
            var valid = ValidTransaction(tx);
            if (!valid.isValid)
            {
                Console.WriteLine(valid.errorMessage);
            }
            return tx;
        }

        public (bool isValid, string errorMessage) ValidTransaction(Transaction transaction)
        {
            if (string.IsNullOrWhiteSpace(transaction.From))
            {
                return (false, "Sender is required.");
            }

            if (string.IsNullOrWhiteSpace(transaction.To))
            {
                return (false, "Recipient is required.");
            }

            if (transaction.Amount <= 0)
            {
                return (false, "Transaction amount must be greater than zero.");
            }       

            return (true, string.Empty);
        }
    }
}
