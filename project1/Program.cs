using System;
using System.IO;

class DiskStorage
{
    private int blockSize = 200;
    private static int diskCapacity = 100 * 1024 * 1024; // 100 MB
    private byte[] memory = new byte[diskCapacity];

    // Read a block of data from memory
    public byte[] ReadBlock(int blockNumber)
    {
        Console.WriteLine("Reading block {0}", blockNumber);
        int offset = blockNumber * blockSize;
        int count = Math.Min(blockSize, memory.Length - offset);

        byte[] data = new byte[count];
        Array.Copy(memory, offset, data, 0, count);

        return data;
    }

    // Write a block of data to memory
    public void WriteBlock(int blockNumber, byte[] data)
    {   
        // Console.WriteLine("Writing to block {0} with data size {1} bytes", blockNumber, data.Length);
        int offset = blockNumber * blockSize;
        int count = Math.Min(blockSize, memory.Length - offset);

        //if data is not long enough, fill the rest with 0
        if (data.Length < count)
        {
            byte[] newData = new byte[count];
            Array.Copy(data, newData, data.Length);
            data = newData;
        }

        Array.Copy(data, 0, memory, offset, count);
    }

    // Load the file data.tsv into memory
    public void LoadFile(string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {   
            Console.WriteLine("Loading file name: {0}", filePath);
            Console.WriteLine("File size: {0} bytes", fileStream.Length);
            Console.WriteLine("Block size: {0} bytes", blockSize);

            int blockCount = (int)Math.Ceiling((double)fileStream.Length / blockSize);
    
            Console.WriteLine("Block count: {0}", blockCount);

            for (int i = 0; i < blockCount; i++)
            {
                int offset = i * blockSize;
                int count = (int)Math.Min(blockSize, fileStream.Length - offset);
                byte[] data = new byte[count];
                fileStream.Read(data, 0, count);
                
                
                WriteBlock(i, data);
            }
        }
    }
}

// class Program2
// {
//     static void Main(string[] args)
//     {
//         DiskStorage disk = new DiskStorage();
//         disk.LoadFile("data.tsv");

//         // TESTING: read any block you want
//         byte[] data = disk.ReadBlock(91269);
//         Console.WriteLine("Read result: {0}", System.Text.Encoding.UTF8.GetString(data));
//     }
// }
