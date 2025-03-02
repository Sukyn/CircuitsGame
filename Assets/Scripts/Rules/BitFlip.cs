using UnityEngine;

public class BitFlip : Rule
{
    protected override NodeType[,] Left()
    {
        NodeType[,] nodesGrid = new NodeType[2, 1];
        nodesGrid[0, 0] = NodeType.X;
        nodesGrid[1, 0] = NodeType.X;
        return nodesGrid;
    }

    protected override NodeType[,] Right()
    {
        NodeType[,] nodesGrid = new NodeType[2, 1];
        nodesGrid[0, 0] = NodeType.Empty;
        nodesGrid[1, 0] = NodeType.Empty;
        return nodesGrid;
    }
}

