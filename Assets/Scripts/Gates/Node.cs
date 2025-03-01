using UnityEngine;

public class Node : MonoBehaviour
{
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


    void OnValidate()
    {
        Cache();
        UpdateColor();
    }

    void UpdateColor()
    {
        if (type == NodeType.X || type == NodeType.X2)
            spriteRenderer.color = isSelected ? new Color(1, 0.5f, 0.5f) : new Color(1, 0, 0);

        else if (type == NodeType.Z || type == NodeType.Z2)
            spriteRenderer.color = isSelected ? new Color(0.5f, 1, 0.5f) : new Color(0, 1, 0);

        else if (type == NodeType.H)
            spriteRenderer.color = isSelected ? new Color(1, 1f, 0.5f) : new Color(1, 1, 0);
    }

    void Start() {
        Cache();
    }

    void Cache()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void OnMouseDown()
    {
        isSelected = !isSelected;
        UpdateColor();
        //Debug.Log(gameObject.name + (isSelected ? " sélectionné !" : " désélectionné !"));
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
