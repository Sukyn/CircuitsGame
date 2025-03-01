using UnityEngine;

public class Intrication : Rule
{
    protected override Node.NodeType[,] Left()
    {
        Node.NodeType[,] nodesGrid = new Node.NodeType[2, 2];
        nodesGrid[0, 0] = Node.NodeType.ZUp;
        nodesGrid[1, 0] = Node.NodeType.ZUp;
        nodesGrid[0, 1] = Node.NodeType.ZDown;
        nodesGrid[1, 1] = Node.NodeType.ZDown;
        return nodesGrid;
    }

    protected override Node.NodeType[,] Right()
    {
        Node.NodeType[,] nodesGrid = new Node.NodeType[2, 2];
        nodesGrid[0, 0] = Node.NodeType.Empty;
        nodesGrid[1, 0] = Node.NodeType.Empty;
        nodesGrid[0, 1] = Node.NodeType.Empty;
        nodesGrid[1, 1] = Node.NodeType.Empty;
        return nodesGrid;
    }
}

