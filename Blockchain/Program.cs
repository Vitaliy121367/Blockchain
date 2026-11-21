using Blockchain.Models;
using Blockchain.Service;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Blockchain
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var blockChainService = await BlockChain.CreateAsync();
            var displayService = new DisplayService();
            var tempListTransaction = new List<Transaction>();
            var transactionService = new TransactionService();

            var walletService = new WalletService();

            List<Wallet> wallets = new List<Wallet>();

            while (true)
            {
                Console.WriteLine("1. Add Block");
                Console.WriteLine("2. Add Transaction");
                Console.WriteLine("3. Display Chain");
                Console.WriteLine("4. Display Transaction");
                Console.WriteLine("5. Chain Valid");
                Console.WriteLine("6. Add 20 Blocks");
                Console.WriteLine("7. Create Wallet");
                Console.WriteLine("8. Display Wallets");
                Console.WriteLine("0. Exit");

                var enter = Console.ReadLine();
                Console.Clear();
                switch (enter)
                {
                    case "1":
                        await blockChainService.AddBlock(tempListTransaction, "author");
                        tempListTransaction.Clear();
                        break;

                    case "2":
                        Console.Write("Enter wallet id from: ");
                        int from = Int32.Parse(Console.ReadLine());
                        Console.Write("Enter wallet id to: ");
                        int to = Int32.Parse(Console.ReadLine());
                        Console.Write("Enter amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        Console.Write("Enter wallet id: ");
                        int walletId = Int32.Parse(Console.ReadLine());
                        tempListTransaction.Add(transactionService.CreateTransaction(wallets[from].Address, wallets[to].Address, amount, wallets[walletId]));
                        break;

                    case "3":
                        displayService.DisplayChain(blockChainService);
                        break;

                    case "4":
                        foreach (var item in tempListTransaction)
                        {
                            Console.WriteLine(item);
                        }
                        break;

                    case "5":
                        Console.WriteLine(blockChainService.IsChainValid());
                        break;

                    case "6":
                        for (int i = 0; i < 20; i++)
                        {
                            await blockChainService.AddBlock(tempListTransaction, $"author {i + 1}");
                            tempListTransaction.Clear();
                        }
                        break;

                    case "7":
                        Console.Write("Enter name for wallet: ");
                        var nameForWallet = Console.ReadLine();
                        var newWallet = walletService.CreateWallet(nameForWallet);
                        Console.WriteLine($"newWallet name: {newWallet.Address}");
                        wallets.Add(newWallet);
                        break;

                    case "8":
                        for (int i = 0; i < wallets.Count; i++)
                        {
                            Console.WriteLine($"Wallet id {i}, Name: {wallets[i].Name}, Address: {wallets[i].Address}");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Error");
                        break;
                }
            }
        }
    }
}