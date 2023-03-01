using System.Diagnostics;

class BruteForceAlgo
{
    private Disk disk;

    public BruteForceAlgo(Disk disk)
    {
        this.disk = disk;
    }

    public int queryEqual(int numVotes)
    {   
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int countRecordEqual = 0;
        int countDataBlockAccessed = 0;

        for (int i = 0; i < disk.getBlocks().Count; i++)
        {
            List<Block> blockList = disk.getBlocks();
            List<Record> recordList = blockList[i].getRecords();
            countDataBlockAccessed += 1;
            for (int j = 0; j < recordList.Count; j++)
            {
                Record record = recordList[j];
                if (record.getNumVotes() == numVotes)
                {
                    countRecordEqual += 1;
                }
            }
        }

        Console.WriteLine("Brute Force Data Block Accessed Count: " + countDataBlockAccessed.ToString());
        stopwatch.Stop();
        Console.WriteLine("Brute Force Time Elapsed: {0}", stopwatch.Elapsed);

        return countRecordEqual;
    }

    public int queryRange(int numVotes1, int numVotes2)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int countRecordInRange = 0;
        int countDataBlockAccessed = 0;

        for (int i = 0; i < disk.getBlocks().Count; i++)
        {
            List<Block> blockList = disk.getBlocks();
            List<Record> recordList = blockList[i].getRecords();
            countDataBlockAccessed += 1;
            for (int j = 0; j < recordList.Count; j++)
            {
                Record record = recordList[j];
                if (record.getNumVotes() >= numVotes1 & record.getNumVotes() <= numVotes2)
                {
                    countRecordInRange += 1;
                }
            }
        }

        Console.WriteLine("Brute Force Data Block Accessed Count: " + countDataBlockAccessed.ToString());
        stopwatch.Stop();
        Console.WriteLine("Brute Force Time Elapsed: {0}", stopwatch.Elapsed);

        return countRecordInRange;
    }

    public void deleteEqual(int numVotes)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int countDataBlockAccessed = 0;
        int countRecordDeleted = 0;
        for (int i = 0; i < disk.getBlocks().Count; i++)
        {
            List<Block> blockList = disk.getBlocks();
            List<Record> recordList = blockList[i].getRecords();
            countDataBlockAccessed += 1;
            for (int j = 0; j < recordList.Count; j++)
            {
                Record record = recordList[j];
                if (record.getNumVotes() == numVotes)
                {
                    recordList.RemoveAt(j);
                    countRecordDeleted += 1;
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine("Brute Force Data Block Accessed Count: " + countDataBlockAccessed.ToString());
        Console.WriteLine(countRecordDeleted.ToString() + " records deleted");
        stopwatch.Stop();
        Console.WriteLine("Brute Force Time Elapsed: {0}", stopwatch.Elapsed);
    }

}