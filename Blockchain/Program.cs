using Blockchain.Models;
using Blockchain.Service;
using System.Diagnostics;

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

            while (true)
            {
                Console.WriteLine("1. Add Block");
                Console.WriteLine("2. Add Transaction");
                Console.WriteLine("3. Display Chain");
                Console.WriteLine("4. Display Transaction");
                Console.WriteLine("5. Chain Valid");
                Console.WriteLine("6. Add 20 Blocks");
                Console.WriteLine("7. Add 4 Block and 12 Transaction");
                Console.WriteLine("8. Find Transaction by Id");
                Console.WriteLine("9. Find Transaction by User");
                Console.WriteLine("10. Test AddBlock");
                Console.WriteLine("11. Test AuditBlockchain");
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
                        Console.Write("Enter from: ");
                        var from = Console.ReadLine();
                        Console.Write("Enter to: ");
                        var to = Console.ReadLine();
                        Console.Write("Enter amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        tempListTransaction.Add(transactionService.CreateTransaction(from, to, amount));
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
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                var transactions = new List<Transaction>
                                {
                                    transactionService.CreateTransaction("Alice", "Bob", 100 + i),
                                    transactionService.CreateTransaction("Bob", "Charlie", 200 + i),
                                    transactionService.CreateTransaction("Charlie", "David", 300 + i)
                                };

                                await blockChainService.AddBlock(transactions, $"author {i + 1}");
                            }

                            break;
                        }

                    case "8":
                        Console.Write("Enter Id: ");
                        var id = Console.ReadLine();
                        blockChainService.FindTransactionById(id);
                        break;

                    case "9":
                        Console.Write("Enter User: ");
                        var user = Console.ReadLine();
                        Console.Write("Enter filter(all, from, to): ");
                        var filter = Console.ReadLine();
                        var transactionsByUser = blockChainService.FindTransactionByUser(user, filter);
                        foreach (var item in transactionsByUser)
                        {
                            Console.WriteLine(item);
                        }
                        break;

                    case "10":
                        {
                            Console.WriteLine("========== TEST 1 ==========");
                            Console.WriteLine("Test: Add valid transactions");

                            int before = blockChainService.Chain.Count;

                            var transactions1 = new List<Transaction>
                            {
                                transactionService.CreateTransaction("Alice", "Bob", 100),
                                transactionService.CreateTransaction("Bob", "Charlie", 200),
                                transactionService.CreateTransaction("Charlie", "David", 300)
                            };

                            await blockChainService.AddBlock(transactions1, "Test Author");

                            Console.WriteLine($"Blocks before: {before}");
                            Console.WriteLine($"Blocks after: {blockChainService.Chain.Count}");
                            Console.WriteLine($"Result: {(blockChainService.Chain.Count == before + 1 ? "PASSED" : "FAILED")}");


                            Console.WriteLine("\n========== TEST 2 ==========");
                            Console.WriteLine("Test: One invalid transaction out of five");

                            before = blockChainService.Chain.Count;

                            var transactions2 = new List<Transaction>
                            {
                                transactionService.CreateTransaction("Alice", "Bob", 100),
                                transactionService.CreateTransaction("Bob", "Charlie", 200),
                                transactionService.CreateTransaction("", "David", 300),
                                transactionService.CreateTransaction("David", "Eve", 400),
                                transactionService.CreateTransaction("Eve", "Alice", 500)
                            };

                            await blockChainService.AddBlock(transactions2, "Test Author");

                            Console.WriteLine($"Blocks before: {before}");
                            Console.WriteLine($"Blocks after: {blockChainService.Chain.Count}");
                            Console.WriteLine($"Result: {(blockChainService.Chain.Count == before ? "PASSED" : "FAILED")}");


                            Console.WriteLine("\n========== TEST 3 ==========");
                            Console.WriteLine("Test: Duplicate transaction ID inside block");

                            before = blockChainService.Chain.Count;

                            var duplicateTransaction = transactionService.CreateTransaction("Alice", "Bob", 100);

                            var transactions3 = new List<Transaction>
                            {
                                duplicateTransaction,
                                duplicateTransaction,
                                transactionService.CreateTransaction("Charlie", "David", 300)
                            };

                            await blockChainService.AddBlock(transactions3, "Test Author");

                            Console.WriteLine($"Blocks before: {before}");
                            Console.WriteLine($"Blocks after: {blockChainService.Chain.Count}");
                            Console.WriteLine($"Result: {(blockChainService.Chain.Count == before ? "PASSED" : "FAILED")}");


                            Console.WriteLine("\n========== TEST 4 ==========");
                            Console.WriteLine("Test: Transaction ID already exists in Blockchain");

                            before = blockChainService.Chain.Count;

                            var existingTransaction = blockChainService.Chain
                                .SelectMany(b => b.Transaction)
                                .First();

                            var transactions4 = new List<Transaction>
                            {
                                existingTransaction
                            };

                            await blockChainService.AddBlock(transactions4, "Test Author");

                            Console.WriteLine($"Blocks before: {before}");
                            Console.WriteLine($"Blocks after: {blockChainService.Chain.Count}");
                            Console.WriteLine($"Result: {(blockChainService.Chain.Count == before ? "PASSED" : "FAILED")}");

                            Console.WriteLine("\n========== TESTING FINISHED ==========");

                            break;
                        }

                    case "11":
                        {
                            Console.WriteLine("========== TEST: BLOCKCHAIN INTEGRITY ==========");
                            Console.WriteLine("Test: Change transaction Amount in existing block");

                            int blockIndex = blockChainService.Chain.Count - 1;

                            if (blockIndex < 1 || blockChainService.Chain[blockIndex].Transaction.Count == 0)
                            {
                                Console.WriteLine("FAILED: No block with transactions found.");
                                break;
                            }

                            var transaction = blockChainService.Chain[blockIndex].Transaction[0];

                            decimal oldAmount = transaction.Amount;

                            transaction.Amount += 100;

                            Console.WriteLine($"Block: {blockChainService.Chain[blockIndex].Index}");
                            Console.WriteLine($"Transaction ID: {transaction.Id}");
                            Console.WriteLine($"Old Amount: {oldAmount}");
                            Console.WriteLine($"New Amount: {transaction.Amount}");

                            Console.WriteLine("\n========== AUDIT RESULT ==========");

                            bool auditResult = blockChainService.AuditBlockchain();

                            Console.WriteLine(
                                $"Test Result: {(!auditResult ? "PASSED" : "FAILED")}"
                            );

                            break;
                        }

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