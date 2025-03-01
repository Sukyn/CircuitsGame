using UnityEngine;
using System.Collections.Generic;

public class InfiniteGrid : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int initialWidth = 5;
    public int height = 1;
    public float cellSize = 1.1f;

    private List<GameObject> grid = new List<GameObject>();
    private int currentWidth;

    void Start()
    {
        currentWidth = initialWidth;
        GenerateInitialGrid();
    }

    void GenerateInitialGrid()
    {
        for (int x = 0; x < initialWidth; x++)
        {
            AddColumn();
        }
    }

    public void ExpandGrid()
    {
        AddColumn();
        currentWidth++;
    }

    void AddColumn()
    {
        int x = currentWidth;

        for (int y = 0; y < height; y++)
        {
            Vector2 position = new Vector2(x * cellSize, y * cellSize);

            // Sélection aléatoire parmi Xavié, Haictaur, Zeaurraux
            GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            GameObject newObject = Instantiate(selectedPrefab, position, Quaternion.identity);
            newObject.name = $"{selectedPrefab.name} ({x}, {y})";

            grid.Add(newObject);
        }
    }
}
