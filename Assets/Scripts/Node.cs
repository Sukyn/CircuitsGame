using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public static HashSet<Node> selectedNodesSet = new HashSet<Node>();
    public static UnityEvent nodeSelectedEvent = new UnityEvent(); // Invoked each time a node has been selected or deselected

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer horizontalLinkSpriteRenderer;
    [SerializeField] GameObject upLink, downLink;
    public NodeLink leftLink, rightLink;

    private bool isSelected = false;
    [HideInInspector] public Vector2Int gridCoor = Vector2Int.zero;

    [SerializeField] List<Sprite> sprites;


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

    public Node GetLeftNeigbor() => GetNeigbor(-1);

    public Node GetRightNeigbor() => GetNeigbor(1);

    public Node GetNeigbor(int dx)
    {
        Level level = Level.currentLevel;
        Node[,] nodesGrid = level.nodesGrid;

        int x = gridCoor.x + dx;

        if (x < 0 ||
            x >= nodesGrid.GetLength(0))
            return null;

        else return nodesGrid[x, gridCoor.y];
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
            NodeLink.UnselectAllLinks();

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
        if (!spriteRenderer)
            return;

        spriteRenderer.sprite = sprites[(int)type];
        spriteRenderer.color = isSelected ? Color.grey : Color.white;
        horizontalLinkSpriteRenderer.color = (type == NodeType.Empty && isSelected) ? Color.grey : Color.black;


        upLink.SetActive(type == NodeType.XDown || type == NodeType.ZDown);
        downLink.SetActive(type == NodeType.XDown || type == NodeType.ZDown);

        leftLink.gameObject.SetActive(type != NodeType.Empty);
        rightLink.gameObject.SetActive(type != NodeType.Empty);
    }

    void OnMouseDown()
    {
        ToggleIsSelected();
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
