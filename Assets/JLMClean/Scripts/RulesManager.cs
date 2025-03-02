using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    Rule[] rules;

    public GameObject choiceMenu;

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
        rules = GetComponents<Rule>();
    }

    void OnNodeSelected()
    {
        List<Rule> rulesAppliable = RulesAppliable();

        if (rulesAppliable.Count == 1)
        {
            rulesAppliable[0].Apply(Node.SelectedNodesGrid());
            Node.UnselectAllNodes();
        }

        if (rulesAppliable.Count == 2)
        {
            choiceMenu.SetActive(true);

            Node[] selectedNodesArray = Node.selectedNodesSet.ToArray();
            Node n0 = selectedNodesArray[0];
            Node n1 = selectedNodesArray[1];
            choiceMenu.transform.position = (n0.transform.position + n1.transform.position) / 2;
        }

        else choiceMenu.SetActive(false);
    }

    List<Rule> RulesAppliable()
    {
        List<Rule> rulesAppliable = new List<Rule>();

        foreach (Rule rule in rules)
            if (rule.IsAppliable(Node.SelectedNodesGrid()))
                rulesAppliable.Add(rule);

        return rulesAppliable;
    }
}
