using UnityEngine;

public class Identite : Rule
{
    public override bool IsValide(Node[,] nodesGrid)
    {
        if (nodesGrid.GetLength(0) != 2 ||
            nodesGrid.GetLength(1) != 1)
            return false;

        if ((nodesGrid[0, 0].type == Node.NodeType.Empty && nodesGrid[1, 0].type == Node.NodeType.H) ||
            (nodesGrid[0, 0].type == Node.NodeType.Empty && nodesGrid[1, 0].type == Node.NodeType.X) ||
            (nodesGrid[0, 0].type == Node.NodeType.H && nodesGrid[1, 0].type == Node.NodeType.Empty) ||
            (nodesGrid[0, 0].type == Node.NodeType.X && nodesGrid[1, 0].type == Node.NodeType.Empty))
            return true;

        return false;
    }

    public override void Apply(Node[,] nodesGrid)
    {
        if (!IsValide(nodesGrid))
        {
            Debug.LogError("!IsValide(nodesGrid)");
            return;
        }

        Node.NodeType type00Cached = nodesGrid[0, 0].type;
        nodesGrid[0, 0].SetType(nodesGrid[1, 0].type);
        nodesGrid[1, 0].SetType(type00Cached);
    }
}

