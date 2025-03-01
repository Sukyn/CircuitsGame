using UnityEngine;

public class Quantique : Rule
{
    protected override Node.NodeType[,] Left()
    {
        Node.NodeType[,] nodesGrid = new Node.NodeType[2, 1];
        nodesGrid[0, 0] = Node.NodeType.H;
        nodesGrid[1, 0] = Node.NodeType.H;
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

