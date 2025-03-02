using UnityEngine;
using UnityEngine.UI;

public abstract class Rule : MonoBehaviour
{
    protected abstract NodeType[,] Left();
    protected abstract NodeType[,] Right();


    public void ApplyThanUnselectAllNodes()
    {
        Apply(Node.SelectedNodesGrid());
        Node.UnselectAllNodes();
    }

    public void Apply(Node[,] nodesGrid)
    {
        if (!IsAppliable(nodesGrid))
        {
            Debug.LogError("!IsAppliable(nodesGrid)");
            return;
        }

        SetNodesGridTypes(nodesGrid, NodesGridMatchTypesGrid(nodesGrid, Left()) ? Right() : Left());

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/swordwind") as AudioClip, Vector3.zero);

        Level.currentLevel.CheckEnd();
    }

    public bool IsAppliable(Node[,] nodesGrid)
    {
        return NodesGridMatchTypesGrid(nodesGrid, Left()) || NodesGridMatchTypesGrid(nodesGrid, Right());
    }

    public bool NodesGridMatchTypesGrid(Node[,] nodesGrid, NodeType[,] typesGrid)
    {
        if (nodesGrid.GetLength(0) != typesGrid.GetLength(0) ||
            nodesGrid.GetLength(1) != typesGrid.GetLength(1))
            return false;

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                if (!nodesGrid[x, y] || nodesGrid[x, y].type != typesGrid[x, y])
                    return false;

        return true;
    }

    public void SetNodesGridTypes(Node[,] nodesGrid, NodeType[,] typesGrid)
    {
        if (nodesGrid.GetLength(0) != typesGrid.GetLength(0) ||
            nodesGrid.GetLength(1) != typesGrid.GetLength(1))
        {
            Debug.LogError("SetNodesGridTypes : Lengths don't match");
            return;
        }

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                nodesGrid[x, y]?.SetType(typesGrid[x, y]);
    }
}
