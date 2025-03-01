using UnityEngine;

public class Node : MonoBehaviour
{
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    public NodeType type;
    public enum NodeType {
        X,
        H,
        Z,
        X2,
        Z2
    }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseDown() {
        if (isSelected) {

            spriteRenderer.color = originalColor;
            isSelected = false;
            Debug.Log(gameObject.name + " désélectionné !");

        } else {
            spriteRenderer.color = Color.green;
            isSelected = true;
            Debug.Log(gameObject.name + " sélectionné !");
        }
    }
}
