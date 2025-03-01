using UnityEngine;
using UnityEngine.UI;

public abstract class Rule : MonoBehaviour
{
    protected abstract Node.NodeType[,] Left();
    protected abstract Node.NodeType[,] Right();

    Button button;
    Rule[] rules;

    bool IsFirstRule() => rules[0] == this;

    void Awake()
    {
        Cache();
    }

    void Start()
    {
        if (IsFirstRule())
            Node.nodeSelectedEvent.AddListener(OnNodeSelected);
    }

    void Cache()
    {
        rules = GetComponents<Rule>();

        button = GetComponent<Button>();

        if (IsFirstRule())
        {
            button.interactable = false;
            button.onClick.AddListener(OnClick);
        }
    }

    void OnNodeSelected()
    {

        button.interactable = AtLeastOneRuleAppliable();
    }

    bool AtLeastOneRuleAppliable() => FirstRuleAppliable() != null;

    Rule FirstRuleAppliable()
    {
        foreach (Rule rule in rules)
            if (rule.IsAppliable(Node.SelectedNodesGrid()))
                return rule;

        return null;
    }

    void OnClick()
    {
        FirstRuleAppliable().Apply(Node.SelectedNodesGrid());
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
                if (!nodesGrid[x, y] || nodesGrid[x, y].type != typesGrid[x, y])
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
