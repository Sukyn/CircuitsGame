using UnityEngine;
using UnityEngine.UI;

public abstract class Rule : MonoBehaviour
{
    protected abstract Node.NodeType[,] Left();
    protected abstract Node.NodeType[,] Right();

    Button button;

    void Awake()
    {
        Cache();
    }

    void Start()
    {
        Node.nodeSelectedEvent.AddListener(OnNodeSelected);
    }

    void Cache()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        button.interactable = false;
    }

    void OnNodeSelected()
    {
        button.interactable = IsAppliable(Node.SelectedNodesGrid());
    }

    void OnClick()
    {
        Apply(Node.SelectedNodesGrid());
        Node.UnselectAllNodes();
    }

    void Apply(Node[,] nodesGrid)
    {
        if (!IsAppliable(nodesGrid))
        {
            Debug.LogError("!IsValide(nodesGrid)");
            return;
        }

        SetNodesGridTypes(nodesGrid, NodesGridMatchTypesGrid(nodesGrid, Left()) ? Right() : Left());
    }

    bool IsAppliable(Node[,] nodesGrid)
    {
        return NodesGridMatchTypesGrid(nodesGrid, Left()) || NodesGridMatchTypesGrid(nodesGrid, Right());
    }

    bool NodesGridMatchTypesGrid(Node[,] nodesGrid, Node.NodeType[,] typesGrid)
    {
        if (nodesGrid.GetLength(0) != typesGrid.GetLength(0) ||
            nodesGrid.GetLength(1) != typesGrid.GetLength(1))
            return false;

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                if (nodesGrid[x, y].type != typesGrid[x, y])
                    return false;

        return true;
    }

    void SetNodesGridTypes(Node[,] nodesGrid, Node.NodeType[,] typesGrid)
    {
        if (nodesGrid.GetLength(0) != typesGrid.GetLength(0) ||
            nodesGrid.GetLength(1) != typesGrid.GetLength(1))
        {
            Debug.LogError("SetNodesGridTypes : Lengths don't match");
            return;
        }

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                nodesGrid[x, y].SetType(typesGrid[x, y]);
    }
}
