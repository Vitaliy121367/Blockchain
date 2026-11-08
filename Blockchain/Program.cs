namespace Blockchain
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var blockChainService = await BlockChain.CreateAsync(5);
            var displayService = new DisplayService();
            DateTime timeStart = DateTime.Now;

            await blockChainService.AddBlock("Alex register", "Alex");
            await blockChainService.AddBlock("Alex buy 100", "Alex");
            await blockChainService.AddBlock("Bob register", "Bob");
            await blockChainService.AddBlock("Alex send Bob 30", "Alex");
            await blockChainService.AddBlock("Bob buy 50", "Bob");

            DateTime timeEnd = DateTime.Now;

            Console.WriteLine($"Time: {timeEnd - timeStart}");

            displayService.DisplayChain(blockChainService);

            if (blockChainService.IsChainValid())
            {
                Console.WriteLine("Blockchain is valid");
            }
            else
            {
                Console.WriteLine("Blockchain is not valid");
            }
            Console.WriteLine(new string('*', 50));
            blockChainService.Chain[2].Data = "Hacked Data";

            if (blockChainService.IsChainValid())
            {
                Console.WriteLine("Blockchain is valid");
            }
            else
            {
                Console.WriteLine("Blockchain is not valid");
            }
            Console.WriteLine(new string('*', 50));
            await blockChainService.RepairChain(0);
            displayService.DisplayChain(blockChainService); 
            if (blockChainService.IsChainValid())
            {
                Console.WriteLine("Blockchain is valid");
            }
            else
            {
                Console.WriteLine("Blockchain is not valid");
            }
        }
    }
}
