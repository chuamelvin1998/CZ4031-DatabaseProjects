using System.Diagnostics;

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
            newChild != child
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
        var right = new Node();
        right.Keys =
            node.Keys.GetRange(splitIndex, node.Keys.Count - splitIndex);

        right.Records =
            node.Records.GetRange(splitIndex, node.Records.Count - splitIndex);

        node.Keys.RemoveRange(splitIndex, node.Keys.Count - splitIndex);
        node.Records.RemoveRange(splitIndex, node.Records.Count - splitIndex);

        right.next = node.next;
        node.next = right;
        node.IsLeaf = right.IsLeaf = true;

        if (node == _root)
        {
            _root = new Node();
            _root.Keys.Add(right.Keys[0]);
            _root.Children.Add (node);
            _root.Children.Add (right);
            _root.IsLeaf = false;
            node = _root;
        }
        else
        {
            Node parent = FindParent(_root, node);
            int j = parent.Children.IndexOf(node);
            parent.Keys.Insert(j, right.Keys[0]);
            parent.Children[j] = node;
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
            node == null
        )
        {
            return;
        }

        if (node.IsLeaf)
        {
            Console.Write("[ ");
            for (int i = 0; i < node.Keys.Count; i++)
            {
                Console.Write(node.Keys[i] + " ");
            }
            Console.Write("].. ");
        }
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

    public void Query(int start, int end)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
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

        Console.Write("Accessing node: { ");
        for (int i = 0; i < node.Keys.Count; i++)
        {
            Console.Write(node.Keys[i] + " ");
        }
        Console.WriteLine("}");

        int counter = 0;

        List<Record> results = new List<Record>();

        if (node.IsLeaf)
        {
            // go to next node if start key larger than all keys in current node
            if (node.Keys[node.Keys.Count - 1] < start)
            {
                node = node.next;
                Console
                    .WriteLine("Accessing next node because start key larger than all keys...");
                Console.Write("Accessing node: { ");
                for (int i = 0; i < node.Keys.Count; i++)
                {
                    Console.Write(node.Keys[i] + " ");
                }
                Console.WriteLine("}");
                accessedNodes++;
            }

            int index =
                node.Keys.IndexOf(node.Keys.Where(x => x >= start).Min());

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
                    // NOTE: Uncomment this to print results
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
            Console.WriteLine("Total index nodes accessed: " + accessedNodes);

            // Count the number of data blocks accessed
            HashSet<int> dataBlocks = new HashSet<int>();
            for (int i = 0; i < results.Count; i++)
            {
                dataBlocks.Add(results[i].getDataBlockID());
            }
            Console
                .WriteLine("Total data blocks accessed: " + dataBlocks.Count);

            double sum = 0;
            for (int i = 0; i < results.Count; i++)
            {
                sum += results[i].getAverageRating();
            }
            Console
                .WriteLine("Average of averageRating: " + sum / results.Count);
            stopwatch.Stop();
            Console.WriteLine("Time elapsed for query: {0}", stopwatch.Elapsed);
            Console.WriteLine();
            return;
        }

        Console.WriteLine("No records found");
        Console.WriteLine("Total nodes accessed: " + accessedNodes);
        stopwatch.Stop();
        Console.WriteLine("Time elapsed for query: {0}", stopwatch.Elapsed);
        Console.WriteLine();

    }

    public void Delete(int key)
    {
        Console.WriteLine("Deleting key " + key + "...");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Node node = _root;

        while (!node.IsLeaf)
        {
            int i = 0;
            while (i < node.Keys.Count && key > node.Keys[i])
            {
                i++;
            }

            node = node.Children[i];
        }

        int index = node.Keys.IndexOf(key);

        if (index != -1)
        {
            Console.WriteLine("Number of records being deleted: " + node.Records[index].Count);
            node.Keys.RemoveAt (index);
            node.Records.RemoveAt (index);

            // rebalance the tree
            Node parent = FindParent(_root, node);
            while (parent != null)
            {
                // check if number of keys are less than the minimum for a leaf node
                if (node.Keys.Count < (MAX_KEYS + 1) / 2)
                {
                    // find the index of the node in the parent's children
                    int i = parent.Children.IndexOf(node);

                    // if the left sibling has spare keys to borrow
                    if (
                        i > 0 &&
                        parent.Children[i - 1].Keys.Count > (MAX_KEYS + 1) / 2
                    )
                    {
                        Node leftSibling = parent.Children[i - 1];

                        // move the last key from the left sibling to the current node
                        node
                            .Keys
                            .Insert(0,
                            leftSibling.Keys[leftSibling.Keys.Count - 1]);
                        node
                            .Records
                            .Insert(0,
                            leftSibling.Records[leftSibling.Records.Count - 1]);
                        leftSibling.Keys.RemoveAt(leftSibling.Keys.Count - 1);
                        leftSibling
                            .Records
                            .RemoveAt(leftSibling.Records.Count - 1);

                        // update the parent key
                        parent.Keys[i - 1] = node.Keys[0];
                    }
                    // if the right sibling has spare keys to borrow
                    else if (
                        i < parent.Children.Count - 1 &&
                        parent.Children[i + 1].Keys.Count > (MAX_KEYS + 1) / 2
                    )
                    {
                        Node rightSibling = parent.Children[i + 1];

                        node.Keys.Add(rightSibling.Keys[0]);
                        node.Records.Add(rightSibling.Records[0]);
                        rightSibling.Keys.RemoveAt(0);
                        rightSibling.Records.RemoveAt(0);

                        parent.Keys[i] = rightSibling.Keys[0];
                    }
                    // neither sibling has spare keys to borrow
                    else
                    {
                        if (i > 0)
                        {
                            // merge with left sibling
                            Node leftSibling = parent.Children[i - 1];

                            leftSibling.Keys.AddRange(node.Keys);
                            leftSibling.Records.AddRange(node.Records);

                            parent.Keys.RemoveAt(i - 1);
                            parent.Children.RemoveAt (i);
                            node = leftSibling;
                        }
                        else
                        {
                            // merge with right sibling
                            Node rightSibling = parent.Children[i + 1];

                            node.Keys.AddRange(rightSibling.Keys);
                            node.Records.AddRange(rightSibling.Records);

                            parent.Keys.RemoveAt (i);
                            parent.Children.RemoveAt(i + 1);
                        }
                    }
                }

                node = parent;
                parent = FindParent(_root, node);
            }
        }
        stopwatch.Stop();
        Console.WriteLine("Time elapsed for delete: {0}", stopwatch.Elapsed);
    }
}
