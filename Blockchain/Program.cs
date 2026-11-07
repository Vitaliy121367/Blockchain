namespace Blockchain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var blockChainService = new BlockChain();
            var displayService = new DisplayService();

            blockChainService.AddBlock("Alex send Bob 100");
            blockChainService.AddBlock("Bob send Charlie 50");
            blockChainService.AddBlock("Charlie send Dave 25");
            blockChainService.AddBlock("Dave send Eve 10");
            blockChainService.AddBlock("Eve send Frank 5");

            displayService.DisplayChain(blockChainService);
            if (blockChainService.IsChainValid())
            {
                Console.WriteLine("Blockchain is valid");
            }
            else
            {
                Console.WriteLine("Blockchain is not valid");
            }

            Console.WriteLine("*************************");

            blockChainService.Chain[2].Data = "Bob send Charlie 1000";
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
