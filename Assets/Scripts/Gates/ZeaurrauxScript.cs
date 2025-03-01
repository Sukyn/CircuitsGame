using UnityEngine;

public class ZeaurrauxScript : MonoBehaviour
{
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;

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
