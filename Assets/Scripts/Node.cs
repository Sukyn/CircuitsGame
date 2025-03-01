using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public static HashSet<Node> selectedNodesSet = new HashSet<Node>();
    public static UnityEvent nodeSelectedEvent = new UnityEvent(); // Invoked each time a node has been selected or deselected

    [SerializeField] SpriteRenderer spriteRenderer, emptySpriteRenderer;
    [SerializeField] GameObject links;
    [SerializeField] GameObject upLink, downLink;

    private bool isSelected = false;
    public Vector2Int gridCoor = Vector2Int.zero;

    public NodeType type;

    public enum NodeType {
        X,
        H,
        Z,
        XUp,
        XDown,
        ZUp,
        ZDown,
        Empty
    }

    public void SetType(NodeType type)
    {
        this.type = type;
        UpdateSprite();
    }

    void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;

        if (isSelected)
            selectedNodesSet.Add(this);

        if (!isSelected)
            selectedNodesSet.Remove(this);

        nodeSelectedEvent.Invoke();

        UpdateSprite();

    }

    void ToggleIsSelected() => SetIsSelected(!isSelected);

    void OnValidate()
    {
        UpdateSprite();
    }


    void OnEnable()
    {
        if (isSelected)
        {
            selectedNodesSet.Add(this);
            nodeSelectedEvent.Invoke();
        }
    }

    void OnDisable()
    {
        if (isSelected)
        {
            selectedNodesSet.Remove(this);
            nodeSelectedEvent.Invoke();
        }
    }

    void UpdateSprite()
    {
        if (!spriteRenderer ||
            !emptySpriteRenderer)
            return;

        spriteRenderer     .gameObject.SetActive(type != NodeType.Empty);
        emptySpriteRenderer.gameObject.SetActive(type == NodeType.Empty);

        links.SetActive(type != NodeType.Empty);
        upLink.SetActive(type == NodeType.XDown || type == NodeType.ZDown);
        downLink.SetActive(type == NodeType.XUp || type == NodeType.ZUp);

        if (type == NodeType.X ||
            type == NodeType.XUp ||
            type == NodeType.XDown)
            spriteRenderer.color = isSelected ? new Color(1, 0, 0) : new Color(0.7f, 0, 0);

        else if (type == NodeType.Z ||
                 type == NodeType.ZUp ||
                 type == NodeType.ZDown)
            spriteRenderer.color = isSelected ? new Color(0, 1, 0) : new Color(0, 0.7f, 0);

        else if (type == NodeType.H)
            spriteRenderer.color = isSelected ? new Color(1, 1, 0) : new Color(0.7f, 0.7f, 0);

        else if (type == NodeType.Empty)
            emptySpriteRenderer.color = isSelected ? Color.gray : Color.black;
    }

    void OnMouseDown()
    {
        ToggleIsSelected();
        //Debug.Log(gameObject.name + (isSelected ? " sélectionné !" : " désélectionné !"));
    }

    public override string ToString()
    {
        switch (type)
        {
            case NodeType.H: return "H";
            case NodeType.X: return "X";
            case NodeType.Z: return "Z";
            case NodeType.XUp: return "XU";
            case NodeType.XDown: return "XD";
            case NodeType.ZUp: return "ZU";
            case NodeType.ZDown: return "ZD";
            case NodeType.Empty: return ".";
        }

        return "";
    }

    public static Node[,] SelectedNodesGrid()
    {
        int xMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMin = int.MaxValue;
        int yMax = int.MinValue;

        foreach (Node node in selectedNodesSet)
        {
            xMin = Mathf.Min(xMin, node.gridCoor.x);
            xMax = Mathf.Max(xMax, node.gridCoor.x);
            yMin = Mathf.Min(yMin, node.gridCoor.y);
            yMax = Mathf.Max(yMax, node.gridCoor.y);
        }

        Node[,] nodesGrid = new Node[xMax - xMin + 1, yMax - yMin + 1];

        foreach (Node node in selectedNodesSet)
            nodesGrid[node.gridCoor.x - xMin, node.gridCoor.y - yMin] = node;

        return nodesGrid;
    }

    public static void UnselectAllNodes()
    {
        foreach (Node node in selectedNodesSet.ToArray())
            node.SetIsSelected(false);
    }
}
