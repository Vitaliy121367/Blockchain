namespace Blockchain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var blockChainService = new BlockChain(3);
            var displayService = new DisplayService();

            blockChainService.AddBlock("Alex register", "Alex");
            blockChainService.AddBlock("Alex buy 100", "Alex");
            blockChainService.AddBlock("Bob register", "Bob");
            blockChainService.AddBlock("Alex send Bob 30", "Alex");
            blockChainService.AddBlock("Bob buy 50", "Bob");

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
            blockChainService.RepairChain(2);
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
