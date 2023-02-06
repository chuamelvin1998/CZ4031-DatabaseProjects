
class Program2
{
    static void Main(string[] args)
    {
        int blockCounter = 1;
        int lineCounter = 0;


        Disk disk = new Disk();
        Block newBlock = new Block( blockCounter);
        

        foreach (string line in System.IO.File.ReadLines("data.tsv")){  
            string[] subs = line.Split('	');

            if(lineCounter == 0){
                lineCounter++;
                continue;
            }

            Record newRecord = new Record();
            newRecord.setRecordID(lineCounter);
            newRecord.setTConst(subs[0].ToCharArray());
            newRecord.setAverageRating(double.Parse(subs[1]));
            newRecord.setNumVotes(int.Parse(subs[2]));

            if(newBlock.getRecords().Count <= 5){
                newBlock.addNewRecord(newRecord);
            }else{
                newBlock.setBlockSize();
                disk.addNewBlock(newBlock);
                blockCounter++;
                newBlock = new Block(blockCounter);

            }
           
            // each record is 34bytes, each block is 200/34 = 5.8, 5records each
            //   Console.WriteLine(sizeof(char) * 9 + sizeof(double) + 2*sizeof(int));
            lineCounter++;  
        }  

        //experiment 1
        Console.WriteLine("total Blocks: " + blockCounter.ToString());
        Console.WriteLine("total Records: " + lineCounter.ToString());
        Console.WriteLine("size of each Record: " + (sizeof(char) * 9 + sizeof(double) + 2*sizeof(int)).ToString());
        Console.WriteLine("Number of Records in each block: " + (200/(sizeof(char) * 9 + sizeof(double) + 2*sizeof(int))).ToString());

    }
}