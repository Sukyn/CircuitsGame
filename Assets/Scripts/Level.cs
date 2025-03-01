using System.Collections.Generic;
using UnityEngine;




public class Level : MonoBehaviour
{
    static public Level currentLevel;

    [HideInInspector] public Node[,] nodesGrid;

    [SerializeField] Node nodePrefab;

    void Start()
    {
        nodesGrid = BuildNodesGrid();

        if (!UpDownMatch(nodesGrid))
            Debug.LogWarning($"{gameObject.name} : Up Down Don't match");
    }

    void OnEnable()
    {
        currentLevel = this;
    }

    void OnDisable()
    {
        if (currentLevel == this)
            currentLevel = null;
    }

    Node[,] BuildNodesGrid()
    {
        Node[] nodes = transform.GetComponentsInChildren<Node>();

        int xMax = int.MinValue;
        int yMax = int.MinValue;

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            int x =  Mathf.FloorToInt(node.transform.localPosition.x);
            int y = -Mathf.FloorToInt(node.transform.localPosition.y);
            xMax = Mathf.Max(xMax, x);
            yMax = Mathf.Max(yMax, y);
        }

        Node[,] nodesGrid = new Node[xMax + 1, yMax + 1];

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            int x =  Mathf.FloorToInt(node.transform.localPosition.x);
            int y = -Mathf.FloorToInt(node.transform.localPosition.y);
            nodesGrid[x, y] = node;
            node.gridCoor = new Vector2Int(x, y);
        }

        return nodesGrid;
    }

    public override string ToString()
    {
        string str = "";

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
        {
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                str += $"{nodesGrid[x, y]}\t";

            str += "\n";
        }

        return str;
    }

    bool UpDownMatch(Node[,] nodesGrid)
    {
        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                if (((nodesGrid[x, y].type == Node.NodeType.XUp || nodesGrid[x, y].type == Node.NodeType.ZUp) &&
                    !(nodesGrid[x, y+1].type == Node.NodeType.XDown || nodesGrid[x, y+1].type == Node.NodeType.ZDown))
                    ||
                    ((nodesGrid[x, y].type == Node.NodeType.XDown || nodesGrid[x, y].type == Node.NodeType.ZDown) &&
                    !(nodesGrid[x, y-1].type == Node.NodeType.XUp || nodesGrid[x, y-1].type == Node.NodeType.ZUp)))
                    return false;

        return true;
    }

    public void InsertEmptyColumn(int xColumn)
    {
        Node[,] newGrid = new Node[nodesGrid.GetLength(0) + 1, nodesGrid.GetLength(1)];

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
        {
            for (int x = 0; x < xColumn; x++)
                newGrid[x, y] = nodesGrid[x, y];

            Node emptyNode = Instantiate(nodePrefab);
            emptyNode.SetType(Node.NodeType.Empty);
            emptyNode.transform.parent = transform;
            emptyNode.transform.localPosition = new Vector3Int(xColumn, -y, 0);
            emptyNode.gridCoor = new Vector2Int(xColumn, y);
            newGrid[xColumn, y] = emptyNode;

            for (int x = xColumn; x < nodesGrid.GetLength(0); x++)
            {
                newGrid[x+1, y] = nodesGrid[x, y];
                newGrid[x+1, y].gridCoor.x++;
                newGrid[x+1, y].transform.position += Vector3.right;
            }
        }

        nodesGrid = newGrid;

        transform.position += Vector3.left * 0.5f;
    }

    public void RemoveColumn(int xColumn)
    {
        Node.UnselectAllNodes();

        Node[,] newGrid = new Node[nodesGrid.GetLength(0) - 1, nodesGrid.GetLength(1)];

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
        {
            for (int x = 0; x < xColumn; x++)
                newGrid[x, y] = nodesGrid[x, y];

            for (int x = xColumn; x < newGrid.GetLength(0); x++)
            {
                newGrid[x, y] = nodesGrid[x+1, y];
                newGrid[x, y].gridCoor.x--;
                newGrid[x, y].transform.position -= Vector3.right;
            }

            Destroy(nodesGrid[xColumn, y].gameObject);
        }

        nodesGrid = newGrid;

        transform.position += Vector3.right * 0.5f;
    }
}
