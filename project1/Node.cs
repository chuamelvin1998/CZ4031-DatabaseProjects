class Node
{
    public List<int> Keys { get; set; }
    public List<Node> Children { get; set; }
    public bool IsLeaf { get; set; }

    public Node()
    {
        Keys = new List<int>();
        Children = new List<Node>();
        IsLeaf = false;
    }
}