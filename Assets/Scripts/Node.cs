using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer, emptySpriteRenderer;
    [SerializeField] GameObject links;
    [SerializeField] GameObject upLink, downLink;

    private bool isSelected = false;

    public NodeType type;

    public enum NodeType {
        X,
        H,
        Z,
        XUp,
        XDown,
        ZUp,
        ZDown,
        Empty
    }


    void OnValidate()
    {
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (!spriteRenderer ||
            !emptySpriteRenderer)
            return;

        spriteRenderer     .gameObject.SetActive(type != NodeType.Empty);
        emptySpriteRenderer.gameObject.SetActive(type == NodeType.Empty);

        links.SetActive(type != NodeType.Empty);
        upLink.SetActive(type == NodeType.XDown || type == NodeType.ZDown);
        downLink.SetActive(type == NodeType.XUp || type == NodeType.ZUp);

        if (type == NodeType.X ||
            type == NodeType.XUp ||
            type == NodeType.XDown)
            spriteRenderer.color = isSelected ? new Color(1, 0, 0) : new Color(0.7f, 0, 0);

        else if (type == NodeType.Z ||
                 type == NodeType.ZUp ||
                 type == NodeType.ZDown)
            spriteRenderer.color = isSelected ? new Color(0, 1, 0) : new Color(0, 0.7f, 0);

        else if (type == NodeType.H)
            spriteRenderer.color = isSelected ? new Color(1, 1, 0) : new Color(0.7f, 0.7f, 0);

        else if (type == NodeType.Empty)
            emptySpriteRenderer.color = isSelected ? Color.gray : Color.black;
    }

    void OnMouseDown()
    {
        isSelected = !isSelected;
        UpdateSprite();
        //Debug.Log(gameObject.name + (isSelected ? " sélectionné !" : " désélectionné !"));
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

        return "";
    }
}
