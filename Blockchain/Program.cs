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
                Console.WriteLine("4. Add 5 Blocks");
                Console.WriteLine("5. Search block by Id");
                Console.WriteLine("6. Search block by Data");
                Console.WriteLine("7. Hack Data");


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
                        Console.WriteLine(blockChainService.FindFirstInvalidBlock());
                        break;

                    case "4":
                        await blockChainService.AddBlock("Реєстрація нового користувача Alex", "Alex");

                        await blockChainService.AddBlock("Bob придбав товар на суму 500 грн", "Bob");

                        await blockChainService.AddBlock("Bob переказав Alex 100 грн", "Bob");

                        await blockChainService.AddBlock("Alex здійснив покупку товару на суму 1200 грн", "Alex");

                        await blockChainService.AddBlock("Користувач Bob змінив адресу електронної пошти", "Bob");
                        break;

                    case "5":
                        Console.WriteLine("Enter Id: ");
                        int id = Int32.Parse(Console.ReadLine());
                        var blockById = blockChainService.SearchBlockByIndex(id);
                        if (blockById != null)
                        {
                            displayService.DisplyBlock(blockById);
                        }
                        else
                        {
                            Console.WriteLine("Block not found");
                        }                        
                        break;

                    case "6":
                        Console.WriteLine("Enter data: ");
                        string data = Console.ReadLine();
                        var blockByData = blockChainService.SearchBlocksByData(data);
                        if (blockByData != null)
                        {
                            foreach (var item in blockByData)
                            {
                                displayService.DisplyBlock(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Block not found");
                        }
                        
                        break;

                    case "7":
                        blockChainService.Chain[2].Data = "Hack Data";
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