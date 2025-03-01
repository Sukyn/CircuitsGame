using UnityEngine;





public class GameManager : MonoBehaviour
{
    Rule[] rules;

    void Awake()
    {
        Cache();
    }

    void Start()
    {
        Node.nodeSelectedEvent.AddListener(OnSelectedNode);
    }

    void Cache()
    {
        rules = GetComponents<Rule>();
    }

    void OnSelectedNode()
    {
        Node[,] selectedNodesGrid = Node.SelectedNodesGrid();

        foreach (Rule rule in rules)
        {
            if (rule.IsValide(selectedNodesGrid))
            {
                rule.Apply(selectedNodesGrid);
                Node.UnselectAllNodes();
                break;
            }
        }
    }
}
