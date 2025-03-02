using UnityEngine;
using UnityEngine.UI;

public class ShowCredits : MonoBehaviour
{
    public DialogueManager dialogueManager; // Référence au DialogueManager
    public GameObject dialogueCanvas; // Canvas contenant le dialogue

    public void OnButtonClick()
    {
        if (!dialogueCanvas.activeSelf)
        {
            // Affiche le dialogue et commence depuis le début
            dialogueManager.StartDialogue();
        }
    }
}
