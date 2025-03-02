using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    public static HashSet<Node> selectedNodesSet = new HashSet<Node>();
    public static UnityEvent nodeSelectedEvent = new UnityEvent(); // Invoked each time a node has been selected or deselected

    public NodeType type;

    public SpriteRenderer characterSR, characterOutlineSR;
    public Sprite[] characterSprites, characterOutlineSprites;

    public GameObject[] outlines;
    public GameObject[] inserts;
    public GameObject chain;

    bool isSelected = false;

    Animator animator;



    [HideInInspector] public Vector2Int gridCoor = Vector2Int.zero;


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
        UpdateSprites();
    }

    void SetIsSelected(bool isSelected, bool invokeEvent = true)
    {
        this.isSelected = isSelected;

        //if (isSelected)
        //    NodeLink.UnselectAllLinks();

        if (isSelected)
            selectedNodesSet.Add(this);

        if (!isSelected)
            selectedNodesSet.Remove(this);

        animator?.SetBool("IsSelected", isSelected);

        UpdateSprites();

        if (invokeEvent)
            nodeSelectedEvent.Invoke();
    }

    void ToggleIsSelected() => SetIsSelected(!isSelected);

    void OnValidate()
    {
        UpdateSprites();
    }

    void Awake()
    {
        Cache();
    }

    void Start()
    {
        animator.Play("NodeIdle", 0, UnityEngine.Random.value);
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


    void OnMouseDown()
    {
        if (Level.currentLevel.ended)
            return;

        ToggleIsSelected();
    }

    void Cache()
    {
        animator = GetComponent<Animator>();
    }

    void UpdateSprites()
    {
        if (!characterSR ||
            !characterOutlineSR ||
            characterSprites.Length < 8 ||
            characterOutlineSprites.Length < 8)
            return;

        characterSR.sprite = characterSprites[(int)type];
        characterOutlineSR.sprite = characterOutlineSprites[(int)type];

        foreach (GameObject outline in outlines)
            outline?.SetActive(isSelected);

        chain?.SetActive(type == NodeType.XDown || type == NodeType.ZDown);

        foreach (GameObject insert in inserts)
            insert?.SetActive(type != NodeType.Empty);
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
            node.SetIsSelected(false, false);

        nodeSelectedEvent.Invoke();
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

        return "UNKNOWN";
    }
}

public enum NodeType
{
    X,
    H,
    Z,
    XUp,
    XDown,
    ZUp,
    ZDown,
    Empty
}