using UnityEngine;
using System.Collections.Generic;

public class InfiniteGrid : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Xavié, Haictaur, Zeaurraux
    public int initialWidth = 5;   // Largeur de départ
    public int height = 5;         // Hauteur fixe
    public float cellSize = 1.1f;  // Taille des cellules (avec un léger espace)

    private List<List<GameObject>> grid = new List<List<GameObject>>(); // Grille stockée
    private int currentWidth;      // Largeur actuelle de la grille

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

    public void ExpandGrid() // Appelle cette fonction pour ajouter une colonne
    {
        AddColumn();
        currentWidth++;
    }

    void AddColumn()
    {
        int x = currentWidth; // Nouvelle colonne
        List<GameObject> columnObjects = new List<GameObject>();

        for (int y = 0; y < height; y++)
        {
            Vector2 position = new Vector2(x * cellSize, y * cellSize);

            // Sélection aléatoire parmi Xavié, Haictaur, Zeaurraux
            GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            GameObject newObject = Instantiate(selectedPrefab, position, Quaternion.identity);
            newObject.name = $"{selectedPrefab.name} ({x}, {y})"; // Nom unique

            columnObjects.Add(newObject);
        }

        grid.Add(columnObjects);
    }
}
