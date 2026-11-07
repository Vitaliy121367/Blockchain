namespace Blockchain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var blockChainService = new BlockChain();
            var displayService = new DisplayService();

            blockChainService.AddBlock("Alex", "Alex register");
            blockChainService.AddBlock("Alex", "Alex buy 100");
            blockChainService.AddBlock("Bob", "Bob register");
            blockChainService.AddBlock("Alex", "Alex send Bob 30");
            blockChainService.AddBlock("Bob", "Bob buy 50");

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

            var blockByIndex = blockChainService.SearchBlockByIndex(3);
            displayService.DisplyBlock(blockByIndex);

            Console.WriteLine(new string('*', 50));

            var SearchBlocksByData = blockChainService.SearchBlocksByData("register");
            foreach (var block in SearchBlocksByData)
            {
                displayService.DisplyBlock(block);
            }
        }
    }
}
