using UnityEngine;

public class BitFlip : Rule
{
    protected override Node.NodeType[,] Left()
    {
        Node.NodeType[,] nodesGrid = new Node.NodeType[2, 1];
        nodesGrid[0, 0] = Node.NodeType.X;
        nodesGrid[1, 0] = Node.NodeType.X;
        return nodesGrid;
    }

    protected override Node.NodeType[,] Right()
    {
        Node.NodeType[,] nodesGrid = new Node.NodeType[2, 1];
        nodesGrid[0, 0] = Node.NodeType.Empty;
        nodesGrid[1, 0] = Node.NodeType.Empty;
        return nodesGrid;
    }
}

