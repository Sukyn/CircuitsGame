using UnityEngine;

public class Intrication : Rule
{
    protected override NodeType[,] Left()
    {
        NodeType[,] nodesGrid = new NodeType[2, 2];
        nodesGrid[0, 0] = NodeType.ZUp;
        nodesGrid[1, 0] = NodeType.ZUp;
        nodesGrid[0, 1] = NodeType.ZDown;
        nodesGrid[1, 1] = NodeType.ZDown;
        return nodesGrid;
    }

    protected override NodeType[,] Right()
    {
        NodeType[,] nodesGrid = new NodeType[2, 2];
        nodesGrid[0, 0] = NodeType.Empty;
        nodesGrid[1, 0] = NodeType.Empty;
        nodesGrid[0, 1] = NodeType.Empty;
        nodesGrid[1, 1] = NodeType.Empty;
        return nodesGrid;
    }
}

