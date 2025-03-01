using UnityEngine;
using UnityEngine.UI;

public class AddEmptyButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        Cache();
    }

    void Start()
    {
        NodeLink.linkSelectedEvent.AddListener(OnLinkSelected);
    }

    void Cache()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        int xColumn = NodeLink.selectedLinksList[0].node.gridCoor.x;

        if (!NodeLink.selectedLinksList[0].isLeft)
            xColumn++;

        Level.currentLevel.InsertEmptyColumn(xColumn);

        NodeLink.UnselectAllLinks();
    }

    void OnLinkSelected()
    {
        button.interactable = NodeLink.selectedLinksList.Count > 0;
    }
}
