using System.Diagnostics;

namespace Blockchain
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var blockChainService = await BlockChain.CreateAsync();
            var displayService = new DisplayService();

            while (true)
            {
                Console.WriteLine("1. Add Block");
                Console.WriteLine("2. Display Chain");
                Console.WriteLine("3. Validate Chain");
                Console.WriteLine("4. Add 8 Blocks");
                Console.WriteLine("5. Print Difficulty History");
                Console.WriteLine("6. Hack data");
                Console.WriteLine("7. Find First Invalid Block");
                Console.WriteLine("8. Hack Hash");

                var enter = Console.ReadLine();
                Console.Clear();
                var startTime = Stopwatch.StartNew();

                switch (enter)
                {
                    case "1":
                        await blockChainService.AddBlock("Bob Send Alex 100", "Bob");
                        break;

                    case "2":
                        displayService.DisplayChain(blockChainService);
                        break;

                    case "3":
                        Console.WriteLine(blockChainService.IsChainValid());
                        break;

                    case "4":
                        for (int i = 0; i < 8; i++)
                        {
                            await blockChainService.AddBlock($"Block {i + 1}", $"Autor {i + 1}");
                        }
                        break;

                    case "5":
                        blockChainService.PrintDifficultyHistory();
                        break;

                    case "6":
                        blockChainService.Chain[2].Data = "Hack data";
                        break;

                    case "7":
                        Console.WriteLine(blockChainService.FindFirstInvalidBlock());
                        break;

                    case "8":
                        blockChainService.Chain[3].Hash = "Hack Hash";
                        break;

                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                startTime.Stop();
                Console.WriteLine($"Time Async: {(decimal)startTime.Elapsed.TotalSeconds}");
            }
        }
    }
}