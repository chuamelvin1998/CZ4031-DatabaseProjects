public struct Block{
        private List<Record> records;
        private int blockID;
        private int blockSize;



    public Block(List<Record> records, int blockSize, int blockID){
            this.records = records;
            this.blockSize = blockSize;
            this.blockID = blockID;
    }
    public Block(int blockID){
        List<Record> emptyRecords = new List<Record>();
            this.records = emptyRecords;
            this.blockSize = 0;
            this.blockID = blockID;
    }

    public void addNewRecord(Record newRecord){
        this.records.Add(newRecord);
    }

	public int getBlockSize() {
		return this.blockSize;
	}

	public void setBlockSize(int blockSize) {
		this.blockSize = blockSize;
	}
	public void setBlockSize() {
		int numOfRecords = this.records.Count;
        this.blockSize =  numOfRecords * 34;

	}
	public List<Record> getRecords() {
		return this.records;
	}

	public void setRecords(List<Record> records) {
		this.records = records;
	}
	public int getBlockID() {
		return this.blockID;
	}

	public void setBlockID(int blockID) {
		this.blockID = blockID;
	}

    public void printRecords()
        {   
            int counter = 1;
            foreach (var record in records) 
            {
                Console.WriteLine(blockID.ToString()+ "." + counter.ToString() + "\t" + new string(record.getTConst()) + "\t" + record.getAverageRating() + "\t" + record.getNumVotes() + "\t\t" + record.getRecordID());
                counter++;
            }
        }


    }