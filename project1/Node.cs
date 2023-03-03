class Node
{
    public List<int> Keys { get; set; }
    public List<Node> Children { get; set; }
    public List<List<Record>> Records { get; set; }
    public bool IsLeaf { get; set; }
    
    public Node next { get; set; }

    public Node()
    {
        Keys = new List<int>();
        Children = new List<Node>();
        Records = new List<List<Record>>(); // only leaf nodes store records
        IsLeaf = false;
        next = null;
    }
}