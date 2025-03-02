using UnityEngine;

public class ChgmtBase : Rule
{
    protected override NodeType[,] Left()
    {
        NodeType[,] nodesGrid = new NodeType[3, 1];
        nodesGrid[0, 0] = NodeType.H;
        nodesGrid[1, 0] = NodeType.Z;
        nodesGrid[2, 0] = NodeType.H;
        return nodesGrid;
    }

    protected override NodeType[,] Right()
    {
        NodeType[,] nodesGrid = new NodeType[3, 1];
        nodesGrid[0, 0] = NodeType.Empty;
        nodesGrid[1, 0] = NodeType.X;
        nodesGrid[2, 0] = NodeType.Empty;
        return nodesGrid;
    }
}

