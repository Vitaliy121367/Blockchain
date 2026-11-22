using Blockchain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain.Service
{
    public class BalanceService
    {
        public decimal GetBalance(string address, List<Block> chain)
        {
            decimal balance = 0;
            foreach(var block in chain)
            {
                foreach (var trasaction in block.Transaction)
                {
                    if (trasaction.From == address)
                    {
                        balance -= trasaction.Amount;
                    }
                    if(trasaction.To == address)
                    {
                        balance += trasaction.Amount;
                    }
                }
            }
            return balance;
        }
        public bool HasSufficientBalance(string address, decimal amount, List<Block> chain, List<Transaction> mempol)
        {
            var balance = GetBalance(address, chain);
            foreach (var item in mempol)
            {
                if (item.From == address)
                {
                    balance -= item.Amount;
                }
            }
            return balance >= amount;
        }
    }
}
