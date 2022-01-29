using System.Diagnostics;

// Unit tests
Console.WriteLine("Running unit tests");
Tree tree = new Tree();
Debug.Assert(tree.GetBalance() == 0);
Debug.Assert(tree.GetHeight() == -1);
tree.Insert(new int[] { 1, 2 });
tree.PrintTree();
Debug.Assert(tree.Search(2) != null);
Debug.Assert(tree.Search(3) == null);
tree.Insert(3);
tree.PrintTree();
tree.Clear();
tree.Insert(new int[] { 3, 1, 2, 5, 10, 20, 15, 4, 17 });
Debug.Assert(tree.Search(17) != null);
Debug.Assert(tree.GetHeight() == 3);
Debug.Assert(tree.GetBalance() == 0);
tree.PrintTree();
tree.Clear();
tree.Insert(new int[] { 3, 1, 2 });
Debug.Assert(tree.GetHeight() == 1);
Debug.Assert(tree.GetBalance() == 0);
tree.PrintTree();

Console.WriteLine("Unit tests completed");

class Node
{
    public Node(int value)
    {
        this.value = value;
    }

    public int value;
    public int height = 0;
    public Node? left;
    public Node? right;
}

// An implementation of a AVL-tree
class Tree
{
    public Node? root;

    public void Insert(int[] values)
    {
        foreach (int value in values)
        {
            Insert(value);
        }
    }

    public void Insert(int value)
    {
        if (root == null)
        {
            root = new Node(value);
            return;
        }

        root = InsertAt(root, value);
    }

    private Node InsertAt(Node? parent, int value)
    {
        if (parent == null)
        {
            return new Node(value);
        }

        if (value < parent.value)
        {
            parent.left = InsertAt(parent.left, value);
        }
        else
        {
            parent.right = InsertAt(parent.right, value);
        }

        // Update height
        parent.height = GetNewHeight(parent);

        // Rebalance (1 is threshold for normal AVL-tree)
        int balance = GetBalance(parent);
        // Left heavy
        if (balance > 1)
        {
            if (GetBalance(parent.left) >= 0)
            {
                parent = RotateRight(parent);
            }
            else
            {
                parent.left = RotateLeft(parent.left);
                parent = RotateRight(parent);
            }
        }
        // Right heavy
        else if (balance < -1)
        {
            if (GetBalance(parent.right) <= 0)
            {
                parent = RotateLeft(parent);
            }
            else
            {
                parent.right = RotateRight(parent.right);
                parent = RotateLeft(parent);
            }
        }

        return parent;
    }

    private Node RotateLeft(Node node)
    {
        Node savedRight = node.right;
        node.right = savedRight.left; // Move old right's left tree to new right tree
        savedRight.left = node;

        // Update heights
        node.height = GetNewHeight(node);
        savedRight.height = GetNewHeight(savedRight);

        return savedRight;
    }

    private Node RotateRight(Node node)
    {
        Node savedLeft = node.left;
        node.left = savedLeft.right; // Move old left's right tree to new left tree
        savedLeft.right = node;

        // Update heights
        node.height = GetNewHeight(node);
        savedLeft.height = GetNewHeight(savedLeft);

        return savedLeft;
    }

    public int GetHeight()
    {
        return GetHeight(root);
    }

    public int GetHeight(Node? node)
    {
        if (node == null)
        {
            return -1;
        }

        return node.height;
    }

    // Calculates the new height from children
    public int GetNewHeight(Node? node)
    {
        if (node == null)
        {
            return -1;
        }

        return Math.Max(GetHeight(node.left), GetHeight(node.right)) + 1;
    }

    public int GetBalance()
    {
        return GetBalance(root);
    }

    // Positive = left heavy Negative = right heavy
    public int GetBalance(Node? node)
    {
        if (node == null)
        {
            return 0;
        }

        return GetHeight(node.left) - GetHeight(node.right);
    }

    public void Clear()
    {
        root = null;
    }

    public void PrintTree()
    {
        Console.WriteLine("Tree:");
        if (root == null)
        {
            return;
        }

        Queue<Node> currentNodes = new Queue<Node>();
        currentNodes.Enqueue(root);
        Queue<Node> nextNodes = new Queue<Node>();

        while (currentNodes.Count() > 0)
        {
            bool nonNullFound = false;

            // Print all nodes on this level
            while (currentNodes.Count() > 0)
            {
                Node node = currentNodes.Dequeue();

                if (node == null)
                {
                    // Add empty node to be able to print out spaces
                    Console.Write("   ");

                    nextNodes.Enqueue(null);
                    nextNodes.Enqueue(null);
                }
                else
                {
                    nonNullFound = true;
                    Console.Write(node.value < 10 ? node.value + "  " : node.value + " ");

                    nextNodes.Enqueue(node.left);
                    nextNodes.Enqueue(node.right);
                }
            }

            Console.Write("\n");

            if (!nonNullFound)
            {
                break;
            }

            // Prepare for next iteration
            currentNodes = new Queue<Node>(nextNodes);
            nextNodes.Clear();
        }
    }

    public Node? Search(int value)
    {
        if (root == null)
        {
            return null;
        }

        Node? nextNode = root;

        while (nextNode != null)
        {
            if (nextNode.value == value)
            {
                Console.WriteLine("Found node with value {0}", value);
                return nextNode;
            }

            if (value < nextNode.value)
            {
                nextNode = nextNode.left;
            }
            else
            {
                nextNode = nextNode.right;
            }
        }

        return null;
    }
}