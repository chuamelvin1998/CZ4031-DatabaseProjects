using System;
using System.IO;

public class DataExplore{
    public void read(){
        var dataFile = System.IO.File.ReadLines("data.tsv");
        String tconst = "";
        int largestNumVote = 0;
        int largestconstLength = 0;
        int lineCounter = 0;

        foreach(string line in dataFile){
            if(lineCounter == 0){
                lineCounter++;
                continue;
            }

            string[] subs = line.Split('	'); 
            //check for largest tconst length
            if (subs[0].Length > largestconstLength){
                largestconstLength = subs[0].Length;
                tconst = subs[0];
            }

            if(int.Parse(subs[2]) > largestNumVote){
                largestNumVote = int.Parse(subs[2]);
            }
        }
        Console.WriteLine("Largest tconst Length: " + largestconstLength);
        Console.WriteLine("tconst value: " + tconst);
        Console.WriteLine("Largest Num Vote Value: " + largestNumVote);
    }
    
}