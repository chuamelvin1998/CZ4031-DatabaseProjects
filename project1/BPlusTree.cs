class BPlusTree
{
    private Node _root;

    private const int MAX_KEYS = 50;

    public BPlusTree()
    {
        _root = new Node();
        _root.IsLeaf = true;
    }

    public void Insert(int key)
    {
        _root = Insert(_root, key);
    }

    private Node Insert(Node node, int key)
    {
        // If the node is a leaf node, insert the key
        if (node.IsLeaf)
        {
            // Insert the key into the leaf node
            int j = 0;
            while (j < node.Keys.Count && key > node.Keys[j])
            {
                j++;
            }
            node.Keys.Insert (j, key);

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
        Node newChild = Insert(child, key);
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
}
