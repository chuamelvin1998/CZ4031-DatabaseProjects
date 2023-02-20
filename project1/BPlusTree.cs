class BPlusTree
{
    private Node _root;

    private const int MAX_KEYS = 25;

    public BPlusTree()
    {
        _root = new Node();
        _root.IsLeaf = true;
    }

    public void Insert(Record record)
    {
        _root = Insert(_root, record);
    }

    private Node Insert(Node node, Record record)
    {
        // Select index
        int key = record.getNumVotes();

        // If the node is a leaf node, insert the key
        if (node.IsLeaf)
        {
            // Insert the key into the leaf node
            int j = 0;
            while (j < node.Keys.Count && key > node.Keys[j])
            {
                j++;
            }

            // if existing key, add record to list
            if (j < node.Keys.Count && key == node.Keys[j])
            {
                node.Records[j].Add(record);
                return node;
            }
            else
            {
                node.Keys.Insert (j, key);
                node.Records.Insert(j, new List<Record>());
                node.Records[j].Add(record);
            }

            // Split the leaf node if it has more than the maximum number of keys
            if (node.Keys.Count > MAX_KEYS)
            {
                return SplitLeafNode(node);
            }
            return node;
        }

        // If not non leaf node, find the child node to insert the key
        int i = 0;
        while (i < node.Keys.Count && key > node.Keys[i])
        {
            i++;
        }

        Node child = node.Children[i];
        Node newChild = Insert(child, record);
        if (
            newChild != child // if not inserted into direct child
        )
        {
            if (node.Keys.Count > MAX_KEYS)
            {
                return SplitNonLeafNode(node);
            }
        }
        return node;
    }

    private Node SplitLeafNode(Node node)
    {
        int splitIndex = (int) Math.Ceiling(node.Keys.Count / 2.0);
        var left = new Node();
        left.Keys = node.Keys.GetRange(0, splitIndex);
        var right = new Node();
        right.Keys =
            node.Keys.GetRange(splitIndex, node.Keys.Count - splitIndex);
        right.IsLeaf = true;

        left.IsLeaf = right.IsLeaf = true;
        left.Records = node.Records.GetRange(0, splitIndex);
        right.Records =
            node.Records.GetRange(splitIndex, node.Records.Count - splitIndex);

        left.next = right;

        // Set the pointers to the new nodes
        if (node == _root)
        {
            _root = new Node();
            _root.Keys.Add(right.Keys[0]);
            _root.Children.Add (left);
            _root.Children.Add (right);
            _root.IsLeaf = false;
            node = _root;
        }
        else
        {
            Node parent = FindParent(_root, node);
            int j = parent.Children.IndexOf(node);
            parent.Keys.Insert(j, right.Keys[0]);
            parent.Children[j] = left;
            parent.Children.Insert(j + 1, right);
            if (parent.Keys.Count > MAX_KEYS)
            {
                return SplitNonLeafNode(parent);
            }
        }
        return node;
    }

    private Node SplitNonLeafNode(Node node)
    {
        int splitIndex = (int) Math.Ceiling(node.Keys.Count / 2.0);
        var left = new Node();
        left.Keys = node.Keys.GetRange(0, splitIndex);

        var right = new Node();
        right.Keys =
            node.Keys.GetRange(splitIndex, node.Keys.Count - splitIndex);

        left.Children = node.Children.GetRange(0, splitIndex + 1);
        right.Children =
            node
                .Children
                .GetRange(splitIndex, node.Children.Count - splitIndex);

        // Set the pointers to the new nodes
        // if node is root or findparent is null
        if (node == _root || FindParent(_root, node) == null)
        {
            _root = new Node();
            _root.Keys.Add(node.Keys[splitIndex]);
            _root.Children.Add (left);
            _root.Children.Add (right);
            _root.IsLeaf = false;
            node = _root;
        }
        else
        {
            Node parent = FindParent(_root, node);
            int j = parent.Children.IndexOf(node);
            parent.Keys.Insert(j, node.Keys[splitIndex]);
            parent.Children[j] = left;
            parent.Children.Insert(j + 1, right);

            if (parent.Keys.Count > MAX_KEYS)
            {
                return SplitNonLeafNode(parent);
            }
        }
        return node;
    }

    private Node FindParent(Node parent, Node child)
    {
        int i = 0;
        while (i < parent.Children.Count)
        {
            if (parent.Children[i] == child)
            {
                return parent;
            }
            else if (!parent.Children[i].IsLeaf)
            {
                Node temp = FindParent(parent.Children[i], child);
                if (temp != null)
                {
                    return temp;
                }
            }
            i++;
        }
        return null;
    }

    public void Print()
    {
        Print (_root);
    }

    private void Print(Node node)
    {
        if (
            node == null // added null check
        )
        {
            return;
        }

        // if leaf node
        if (node.IsLeaf)
        {
            Console.Write("[ ");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                Console.Write(node.Keys[i] + " ");
            }
            Console.Write("].. ");
        } // Non-leaf node resursive
        else
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                Print(node.Children[i]);
            }

            Console.Write("{ ");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                Console.Write(node.Keys[i] + " ");
            }

            Console.Write("}.. ");
        }
    }

    public void PrintRootNode()
    {
        if (_root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Console.Write("Root node: { ");
        for (int i = 0; i < _root.Keys.Count; i++)
        {
            Console.Write(_root.Keys[i] + " ");
        }
        Console.Write("} ");
        Console.WriteLine();
    }

    public void PrintMaxKeys()
    {
        Console.WriteLine("Max Keys (Parameter n): " + MAX_KEYS);
    }

    public void PrintTotalNodes()
    {
        if (_root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue (_root);

        int totalNodes = 0;

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                Node node = queue.Dequeue();
                totalNodes++;

                if (!node.IsLeaf)
                {
                    for (int j = 0; j < node.Children.Count; j++)
                    {
                        queue.Enqueue(node.Children[j]);
                    }
                }
            }
        }

        Console.WriteLine("Total nodes: " + totalNodes);
    }

    public void PrintNumberOfLevels()
    {
        if (_root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue (_root);

        int numberOfLayers = 0;

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                Node node = queue.Dequeue();

                if (!node.IsLeaf)
                {
                    for (int j = 0; j < node.Children.Count; j++)
                    {
                        queue.Enqueue(node.Children[j]);
                    }
                }
            }
            numberOfLayers++;
        }

        Console.WriteLine("Number of layers: " + numberOfLayers);
    }

    public void PrintLayerByLayer()
    {
        if (_root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue (_root);

        while (queue.Count > 0)
        {
            Console.WriteLine();
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                Node node = queue.Dequeue();

                if (node.IsLeaf)
                {
                    Console.Write("[ ");
                    for (int j = 0; j < node.Keys.Count; j++)
                    {
                        Console.Write(node.Keys[j] + " ");
                    }
                    Console.Write("] ");
                }
                else
                {
                    Console.Write("{ ");
                    for (int j = 0; j < node.Keys.Count; j++)
                    {
                        Console.Write(node.Keys[j] + " ");
                        queue.Enqueue(node.Children[j]);
                    }
                    if (node.Children.Count > node.Keys.Count)
                    {
                        queue.Enqueue(node.Children[node.Keys.Count]);
                    }
                    Console.Write("} ");
                }
            }
        }
    }

    public void Query(int key)
    {
        Query (key, key);
    }

    // write a query function that takes a key and returns all nodes that contains the key
    public void Query(int start, int end)
    {
        Console
            .WriteLine("Querying for keys between " +
            start +
            " and " +
            end +
            "...");
        if (_root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        //find the maximum key that is less than or equal to the start key
        int accessedNodes = 1;
        Node node = _root;
        while (!node.IsLeaf)
        {
            accessedNodes++;

            //print node
            Console.Write("Accessing node: { ");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                Console.Write(node.Keys[i] + " ");
            }
            Console.WriteLine("}");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                if (node.Keys[i] > start)
                {
                    node = node.Children[i];
                    break;
                }
                if (i == node.Keys.Count - 1)
                {
                    node = node.Children[i + 1];
                    break;
                }
            }
        }

        //print node
        Console.Write("Accessing node: { ");
        for (int i = 0; i < node.Keys.Count; i++)
        {
            Console.Write(node.Keys[i] + " ");
        }
        Console.WriteLine("}");

        int counter = 0;

        //list of Node results
        List<Record> results = new List<Record>();

        if (node.Keys.Contains(start) && node.IsLeaf)
        {
            int index = node.Keys.IndexOf(start);

            // loop through subsequent keys throughout subsequent leaf nodes, until end is found
            while (node != null &&
                node.Keys[index] >= start &&
                node.Keys[index] <= end
            )
            {
                List<Record> recordList = node.Records[index];
                for (int j = 0; j < recordList.Count; j++)
                {
                    results.Add(recordList[j]);
                    counter++;
                    // Console.WriteLine(new string(recordList[j].getTConst()) + "\t" + recordList[j].getAverageRating() + "\t" + recordList[j].getNumVotes() + "\t\t" + recordList[j].getRecordID());
                }
                index++;
                if (index >= node.Keys.Count)
                {
                    Node tmp = node;
                    node = node.next;
                    accessedNodes++;
                    index = 0;
                }
            }
            Console.WriteLine("Total records found: " + counter);
            Console.WriteLine("Total nodes accessed: " + accessedNodes);

            double sum = 0;
            for (int i = 0; i < results.Count; i++)
            {
                sum += results[i].getAverageRating();
            }
            Console.WriteLine("Average of averageRating: " + sum / results.Count);
            
            Console.WriteLine();
            return;
        }

        Console.WriteLine("No records found");
        Console.WriteLine("Total nodes accessed: " + accessedNodes);
        Console.WriteLine();
    }
}
