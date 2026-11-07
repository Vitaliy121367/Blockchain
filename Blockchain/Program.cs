namespace Blockchain
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1
            //var blockChainService = new BlockChain();
            //var displayService = new DisplayService();

            //blockChainService.AddBlock("Alex send Bob 100");
            //blockChainService.AddBlock("Bob send Charlie 50");
            //blockChainService.AddBlock("Charlie send Dave 25");
            //blockChainService.AddBlock("Dave send Eve 10");
            //blockChainService.AddBlock("Eve send Frank 5");

            //displayService.DisplayChain(blockChainService);
            //if (blockChainService.IsChainValid())
            //{
            //    Console.WriteLine("Blockchain is valid");
            //}
            //else
            //{
            //    Console.WriteLine("Blockchain is not valid");
            //}

            //Console.WriteLine("*************************");

            //blockChainService.Chain[2].Data = "Bob send Charlie 1000";
            //displayService.DisplayChain(blockChainService);

            //if (blockChainService.IsChainValid())
            //{
            //    Console.WriteLine("Blockchain is valid");
            //}
            //else
            //{
            //    Console.WriteLine("Blockchain is not valid");
            //}

            //2
            var blockChainService = new BlockChain();
            var displayService = new DisplayService();

            blockChainService.AddBlock("Alex register");
            blockChainService.AddBlock("Alex buy 100");
            blockChainService.AddBlock("Bob register");
            blockChainService.AddBlock("Alex send Bob 30");
            blockChainService.AddBlock("Bob buy 50");

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
            foreach(var block in SearchBlocksByData)
            {
                displayService.DisplyBlock(block);
            }
        }
    }
}
