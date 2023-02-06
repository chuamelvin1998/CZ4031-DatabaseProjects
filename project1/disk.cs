public struct Disk
    {
        private List<Block> blocks;

        public Disk(List<Block> blocks){
            this.blocks = blocks;
        }

        public Disk(){
            List<Block> emptyList = new List<Block>();
            this.blocks = emptyList;
        }
        
        public List<Block> getBlocks() {
            return this.blocks;
        }

         public void addNewBlock(Block newblock){
            blocks.Add(newblock);
         }

        public void setBlocks(List<Block> blocks) {
            this.blocks = blocks;
        }

        public int getNumberOfBlocks() 
        {
            return blocks.Count;
        }

        public void printBlockContent(int blockID)
        {
            this.blocks[blockID].printRecords();
        }

    }