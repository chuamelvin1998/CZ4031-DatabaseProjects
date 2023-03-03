public struct Record{
    private char[] tConst;
    private double averageRating;
    private int numVotes;
    private int recordID;

    public Record(char[] tConst, double averageRating, int numVotes, int recordID){
            this.tConst = tConst;
            this.averageRating = averageRating;
            this.numVotes = numVotes;
            this.recordID = 0;
    }

    public void printRecord(){
        string str = new string(getTConst());
        Console.WriteLine("tconst = " + str + ", averageRating = " + getAverageRating()
        + ", numVotes = " + getNumVotes() + " blockID = " + recordID);
            
    }

	public int getBytes(){
            return sizeof(char) * 9 + sizeof(double) + 2*sizeof(int); //2*9 + 8 + 4 + 4= 36
    }

	public char[] getTConst() {
		return this.tConst;
	}

	public void setTConst(char[] tConst) {
		this.tConst = tConst;
	}

	public double getAverageRating() {
		return this.averageRating;
	}

	public void setAverageRating(double averageRating) {
		this.averageRating = averageRating;
	}

	public int getNumVotes() {
		return this.numVotes;
	}

	public void setNumVotes(int numVotes) {
		this.numVotes = numVotes;
	}

	public int getRecordID() {
		return this.recordID;
	}

	public void setRecordID(int recordID) {
		this.recordID = recordID;
	}

	public int getDataBlockID() {
		return (int)Math.Ceiling((double)recordID/5);
	}

    }