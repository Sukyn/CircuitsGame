using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RemoveEmptyButton : MonoBehaviour
{
    Button button;

    private void Awake()
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
        button.interactable = false;
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        HashSet<int> xSet = new HashSet<int>();

        foreach (Node node in Node.selectedNodesSet)
            xSet.Add(node.gridCoor.x);

        int[] xArray = xSet.ToArray();

        foreach (int x in xArray)
        {
            Level.currentLevel.RemoveColumn(x);

            for (int i = 0; i < xArray.Length; i++)
                if (xArray[i] > x)
                    xArray[i]--;
        }
    }

    void OnNodeSelected()
    {
        button.interactable = IsAppliable();
    }

    bool IsAppliable()
    {
        if (Node.selectedNodesSet.Count == 0)
            return false;

        HashSet<int> xSet = new HashSet<int>();

        foreach (Node node in Node.selectedNodesSet)
            xSet.Add(node.gridCoor.x);

        foreach (int x in xSet)
            for (int y = 0; y < Level.currentLevel.nodesGrid.GetLength(1); y++)
                if (Level.currentLevel.nodesGrid[x, y].type != Node.NodeType.Empty)
                    return false;

        return true;
    }
}
