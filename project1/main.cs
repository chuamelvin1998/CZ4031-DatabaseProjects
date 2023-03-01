
using System.Diagnostics;

class Program2
{
    static void Main(string[] args)
    {
        const int HEADER_ROWS = 1;

        int blockCounter = 1;
        int lineCounter = 0;


        Disk disk = new Disk();
        Block newBlock = new Block(blockCounter);
        DataExplore dataExplore = new DataExplore();

        foreach (string line in System.IO.File.ReadLines("data.tsv"))
        {
            string[] subs = line.Split('	');

            if (lineCounter == 0)
            {
                lineCounter++;
                continue;
            }

            Record newRecord = new Record();
            newRecord.setRecordID(lineCounter);
            newRecord.setTConst(subs[0].ToCharArray());
            newRecord.setAverageRating(double.Parse(subs[1]));
            newRecord.setNumVotes(int.Parse(subs[2]));

            if (newBlock.getRecords().Count < 5)
            {
                newBlock.addNewRecord(newRecord);
            }
            else
            {
                newBlock.setBlockSize();
                disk.addNewBlock(newBlock);
                blockCounter++;
                newBlock = new Block(blockCounter);
                newBlock.addNewRecord(newRecord);
            }

            // each record is 36bytes, each block is 200/34 = 5.5, 5records each
            //   Console.WriteLine(sizeof(char) * 10 + sizeof(double) + 2*sizeof(int));
            lineCounter++;
        }

        // if there are any records left in the last block, add it to the disk
        if (newBlock.getRecords().Count > 0 & newBlock.getRecords().Count < 5)
        {
            newBlock.setBlockSize();
            disk.addNewBlock(newBlock);
            blockCounter++;
        }

        //Data exploration
        dataExplore.read();

        //testing
        Block testBlock = disk.getBlocks()[^3];
        testBlock.printRecords();
        Block testBlock1 = disk.getBlocks()[^2];
        testBlock1.printRecords();
        Block testBlock2 = disk.getBlocks()[^1];
        testBlock2.printRecords();

        //experiment 1 - storage
        Console.WriteLine("\nRunning Experiment 1...");
        Console.WriteLine("Number of Records in each block: " + (200 / (sizeof(char) * 9 + sizeof(double) + 2 * sizeof(int))).ToString());
        Console.WriteLine("size of each Record: " + (sizeof(char) * 10 + sizeof(double) + 2 * sizeof(int)).ToString());
        Console.WriteLine("total Records: " + (lineCounter - HEADER_ROWS));
        Console.WriteLine("total Blocks: " + disk.getNumberOfBlocks());
        Console.WriteLine("Finished Experiment 1...\n");

        //experiment 2 - B+ tree
        Console.WriteLine("Running Experiment 2...");
        //measure time
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        BPlusTree bPlusTree = new BPlusTree();

        foreach (Block block in disk.getBlocks())
        {
            foreach (Record record in block.getRecords())
            {
                bPlusTree.Insert(record);
            }
        }

        bPlusTree.PrintMaxKeys();
        bPlusTree.PrintTotalNodes();
        bPlusTree.PrintNumberOfLevels();
        bPlusTree.PrintRootNode();
        stopwatch.Stop();
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

        Console.WriteLine("Finished Experiment 2...\n");

        //experiment 3 - Searching
        BruteForceAlgo bruteForceAlgo = new BruteForceAlgo(disk);
        Console.WriteLine("Running Experiment 3...");
        bPlusTree.Query(500);
        bruteForceAlgo.queryEqual(500);
        Console.WriteLine("Finished Experiment 3...\n");

        //experiment 4 - Ranged Searching

        Console.WriteLine("Running Experiment 4...");
        bPlusTree.Query(30000, 40000);
        bruteForceAlgo.queryRange(30000, 40000);
        Console.WriteLine("Finished Experiment 4...\n");

        //experiment 5 - Deletion
        Console.WriteLine("Running Experiment 5...");
        bPlusTree.Query(1000);
        bruteForceAlgo.deleteEqual(1000);
        Console.WriteLine("Finished Experiment 5...\n");
    }
}
