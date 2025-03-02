using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    static public Level currentLevel;

    [HideInInspector] public Node[,] nodesGrid;
     NodeType[,] initialNodesTypeGrid;

    [SerializeField] Node nodePrefab;

    public bool ended = false;

    void Start()
    {
        nodesGrid = BuildNodesGrid();
        initialNodesTypeGrid = ExtractNodesType(nodesGrid);
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

    NodeType[,] ExtractNodesType(Node[,] nodesGrid)
    {
        NodeType[,] nodesTypeGrid = new NodeType[nodesGrid.GetLength(0), nodesGrid.GetLength(1)];

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                nodesTypeGrid[x, y] = nodesGrid[x, y].type;

        return nodesTypeGrid;
    }

    public void InsertEmptyColumn(int xColumn)
    {
        if (ended ||
            nodesGrid.GetLength(0) >= 8)
            return;

        Node[,] newGrid = new Node[nodesGrid.GetLength(0) + 1, nodesGrid.GetLength(1)];

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
        {
            for (int x = 0; x < xColumn; x++)
                newGrid[x, y] = nodesGrid[x, y];

            Node emptyNode = Instantiate(nodePrefab);
            emptyNode.SetType(NodeType.Empty);
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
        if (ended)
            return;

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

    public void Reset()
    {
        if (ended)
            return;

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                Destroy(nodesGrid[x, y].gameObject);

        nodesGrid = new Node[initialNodesTypeGrid.GetLength(0), initialNodesTypeGrid.GetLength(1)];

        for (int y = 0; y < nodesGrid.GetLength(1); y++)
        {
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
            {
                Node node = Instantiate(nodePrefab);
                node.transform.parent = transform;
                node.transform.localPosition = new Vector3(x, -y, 0);
                node.SetType(initialNodesTypeGrid[x, y]);
                node.gridCoor = new Vector2Int(x, y);
                nodesGrid[x, y] = node;
            }
        }

        transform.position = new Vector3(-nodesGrid.GetLength(0)+1, nodesGrid.GetLength(1)-1, 0) / 2;
    }

    bool GridIsEmpty()
    {
        for (int y = 0; y < nodesGrid.GetLength(1); y++)
            for (int x = 0; x < nodesGrid.GetLength(0); x++)
                if (nodesGrid[x, y].type != NodeType.Empty)
                    return false;

        return true;
    }

    public void CheckEnd()
    {
        if (!GridIsEmpty())
            return;

        ended = true;

        Invoke("LoadNextLevel", 1);
    }

    void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevelIdx = int.Parse(currentSceneName.Substring(5));
        SceneManager.LoadScene($"Level{currentLevelIdx + 1}", LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(currentSceneName);
    }
}
