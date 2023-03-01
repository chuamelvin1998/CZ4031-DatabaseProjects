class BruteForceAlgo
{
    private Disk disk;

    public BruteForceAlgo(Disk disk)
    {
        this.disk = disk;
    }

    public int queryEqual(int numVotes)
    {
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

        Console.WriteLine("Data Block Accessed Count: " + countDataBlockAccessed.ToString());

        return countRecordEqual;
    }

    public int queryRange(int numVotes1, int numVotes2)
    {
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

        Console.WriteLine("Data Block Accessed Count: " + countDataBlockAccessed.ToString());

        return countRecordInRange;
    }

    public void deleteEqual(int numVotes)
    {
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

        Console.WriteLine("Data Block Accessed Count: " + countDataBlockAccessed.ToString());
        Console.WriteLine(countRecordDeleted.ToString() + " records deleted");
    }

}