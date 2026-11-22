using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain.Service
{
    public class TransactionService
    {
        private readonly WalletService _walletService = new WalletService();
        private readonly BalanceService _balanceService = new BalanceService();
        public Transaction? CreateTransaction(string from, string to, decimal amount, Wallet wallet)
        {
            var tx = new Transaction(from, to, amount, wallet.PublicKey);
            tx.Signature = wallet.Sign(tx.GetDataToSign());

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
                return (false, $"Sender is required transaction {transaction.Id}");
            }

            if (string.IsNullOrWhiteSpace(transaction.To))
            {
                return (false, $"Recipient is required transaction {transaction.Id}");
            }

            if (transaction.Amount <= 0)
            {
                return (false, $"Transaction amount must be greater than zero transaction {transaction.Id}");
            }

            if (transaction.PublicKey == null || transaction.PublicKey.Length == 0)
            {
                return (false, $"Public key is null");
            }

            if (transaction.Signature == null || transaction.Signature.Length == 0)
            {
                return (false, $"Signature is null");
            }

            if (!_walletService.VerifySignature(transaction.GetDataToSign(), transaction.Signature, transaction.PublicKey))
            {
                return (false, $"Invalid Signature");
            }
            if (_walletService.GetAddress(transaction.PublicKey) != transaction.From)
            {
                return (false, "Public key does not belong to sender");
            }

            return (true, string.Empty);
        }

        public (bool isValid, string errorMessage) ValidBalance(Transaction transaction, List<Block> chain, List<Transaction> mempol)
        {
            if (transaction.isCoinBase)
            {
                return (true, string.Empty);
            }
            if (!_balanceService.HasSufficientBalance(transaction.From, transaction.Amount, chain, mempol))
            {
                return (false, "Insufficient balance for the transaction.");
            }
            var validationResult = ValidTransaction(transaction);
            if (!validationResult.isValid)
            {
                return validationResult;
            }
            return (true, string.Empty);            
        }
    }
}
