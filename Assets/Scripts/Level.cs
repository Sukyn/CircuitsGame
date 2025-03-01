using System.Collections.Generic;
using UnityEngine;




public class Level : MonoBehaviour
{
    Node[,] nodesGrid;


    void Start()
    {
        nodesGrid = BuildNodesGrid();

        if (!UpDownMatch(nodesGrid))
            Debug.LogWarning($"{gameObject.name} : Up Down Don't match");

        print(this);
    }


    Node[,] BuildNodesGrid()
    {
        Node[] nodes = transform.GetComponentsInChildren<Node>();

        int xMin = int.MaxValue;
        int xMax = int.MinValue;
        int yMin = int.MaxValue;
        int yMax = int.MinValue;

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            int x =  (int)node.transform.position.x;
            int y = -(int)node.transform.position.y;
            xMin = Mathf.Min(xMin, x);
            xMax = Mathf.Max(xMax, x);
            yMin = Mathf.Min(yMin, y);
            yMax = Mathf.Max(yMax, y);
        }

        Node[,] nodesGrid = new Node[xMax - xMin + 1, yMax - yMin + 1];

        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            int x =  (int)node.transform.position.x;
            int y = -(int)node.transform.position.y;
            nodesGrid[x - xMin, y - yMin] = node;
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
}
