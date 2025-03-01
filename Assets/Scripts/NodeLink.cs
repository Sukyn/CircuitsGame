using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class NodeLink : MonoBehaviour
{
    public static List<NodeLink> selectedLinksList = new List<NodeLink>();
    public static UnityEvent linkSelectedEvent = new UnityEvent(); // Invoked each time a link has been selected or deselected

    public bool isLeft = true;

    [HideInInspector] public Node node;
    SpriteRenderer spriteRenderer;

    public Vector2 GridCoor() => new Vector2(node.gridCoor.x + (isLeft ? -0.5f : 0.5f), node.gridCoor.y);

    private bool isSelected = false;

    void SetIsSelected(bool isSelected, bool invokeEvent = true, bool applyToNeigbors = true, bool unselectAllNodesAndLinks = true)
    {
        if (this.isSelected == isSelected)
            return;

        if (unselectAllNodesAndLinks && isSelected)
        {
            UnselectAllLinks();
            Node.UnselectAllNodes();
        }

        this.isSelected = isSelected;

        if (isSelected)
            selectedLinksList.Add(this);

        if (!isSelected)
            selectedLinksList.Remove(this);

        UpdateSpriteRenderer();

        if (applyToNeigbors)
        {
            if (isLeft)
            {
                Node neigbor = node.GetLeftNeigbor();

                if (neigbor && neigbor.type != Node.NodeType.Empty)
                    neigbor.rightLink.SetIsSelected(isSelected, false, false, false);
            }
            else 
            {
                Node neigbor = node.GetRightNeigbor();

                if (neigbor && neigbor.type != Node.NodeType.Empty)
                    neigbor.leftLink.SetIsSelected(isSelected, false, false, false);

            }
        }

        if (invokeEvent)
            linkSelectedEvent.Invoke();
    }

    void ToggleIsSelected() => SetIsSelected(!isSelected);

    void UpdateSpriteRenderer()
    {
        spriteRenderer.color = isSelected ? Color.gray : Color.black;
    }

    void Awake()
    {
        Cache();
    }

    void Cache()
    {
        node = GetComponentInParent<Node>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        ToggleIsSelected();
    }

    public static void UnselectAllLinks()
    {
        foreach (NodeLink link in selectedLinksList.ToArray())
            link.SetIsSelected(false, true, false, false);
    }
}
