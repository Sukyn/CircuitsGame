using UnityEngine;

public class XavieScript : MonoBehaviour
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
            spriteRenderer.color = Color.red;
            isSelected = true;
            Debug.Log(gameObject.name + " sélectionné !");
        }
    }
}
