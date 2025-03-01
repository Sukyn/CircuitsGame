using UnityEngine;

public class GameObjectMatrix : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int initialColumns = 5;
    public int initialRows = 1;
    public float spacing = 1.1f;
    private GameObject[,] matrix;

    void Start()
    {
        matrix = new GameObject[initialRows, initialColumns];
        CreateGrid();
    }

    void CreateGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int col = 0; col < initialColumns; col++) {
            // List<GameObject> prefabs = new List<GameObject>();
            // for(int row = 0; row < rowNumber(); row++) {
            //     GameObject selectedPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            //     prefabs.Add(selectedPrefab);
            // }
            // AddColumn(col, prefabs);
        }
    }

    // Cette méthode permet d'ajouter une colonne à la grille
    public void AddColumn(int columnNumber, GameObject[] prefabs) {

        for (int row = 0; row < initialRows; row++) {
            Vector3 position = new Vector3(columnNumber * spacing, row * spacing, 0);
            GameObject obj = Instantiate(prefabs[row], position, Quaternion.identity);
            obj.transform.parent = transform;
            matrix[row, columnNumber] = obj;
        }
    }

    private int rowNumber() {
        return matrix.Length;
    }

    // private int columnNumber() {
    //     return matrix[0].Length;
    // }
}
