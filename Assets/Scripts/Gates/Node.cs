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
            switch(type) {
                case NodeType.Z:
                    spriteRenderer.color = Color.green;
                    break;
                case NodeType.X:
                    spriteRenderer.color = Color.red;
                    break;
                case NodeType.H:
                    spriteRenderer.color = Color.yellow;
                    break;
            }
            isSelected = true;
            Debug.Log(gameObject.name + " sélectionné !");
        }
    }

    public override string ToString()
    {
        switch (type)
        {
            case NodeType.H: return "H";
            case NodeType.X: return "X";
            case NodeType.Z: return "Z";
            case NodeType.X2: return "X2";
            case NodeType.Z2: return "Z2";
        }

        return "";
    }
}
