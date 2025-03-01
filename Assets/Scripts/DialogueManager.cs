using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class DialogueEntry
{
    public string category;
    public List<string> lines;
}

[System.Serializable]
public class DialogueData
{
    public List<DialogueEntry> dialogues;
}

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBackground;
    public TextMeshProUGUI dialogueText;
    private bool isTalking = false;
    private bool isTyping = false;
    private List<string> dialogues = new List<string>();
    private int currentDialogueIndex = 0;
    public string dialogueSection = "intro";
    public float typingSpeed = 0.02f;

    void Start()
    {
        LoadDialogues(dialogueSection);
        if (dialogues.Count > 0)
        {
            StartCoroutine(ShowDialogue(dialogues[currentDialogueIndex]));
        }
    }

    void Update()
    {
        if (isTalking && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            if (isTyping) {
                // Si l'animation d'écriture est en cours, affiche le texte complet immédiatement
                StopAllCoroutines();
                dialogueText.text = dialogues[currentDialogueIndex];
                isTyping = false;
            } else {
                NextDialogue();
            }
        }
    }

    private IEnumerator ShowDialogue(string text)
    {
        isTalking = true;
        isTyping = true;
        dialogueText.text = "";
        dialogueBackground.SetActive(true);

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void NextDialogue() {
        currentDialogueIndex++;

        if (currentDialogueIndex >= dialogues.Count) {
            CloseDialogue();
        } else {
            StartCoroutine(ShowDialogue(dialogues[currentDialogueIndex]));
        }
    }

    private void CloseDialogue() {
        StopAllCoroutines();
        dialogueText.text = "";
        dialogueBackground.transform.parent.gameObject.SetActive(false);
        isTalking = false;
        isTyping = false;
    }

    private void LoadDialogues(string dialogueCategory)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "dialogues.json");

        if (!File.Exists(filePath)) {
            Debug.LogError("Fichier dialogues.json introuvable !");
            return;
        }

        string jsonContent = File.ReadAllText(filePath);
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);

        if (dialogueData == null || dialogueData.dialogues == null) {
            Debug.LogError("Erreur lors du chargement des dialogues !");
            return;
        }

        // Trouver la bonne catégorie et extraire les lignes
        foreach (DialogueEntry entry in dialogueData.dialogues) {
            if (entry.category == dialogueCategory) {
                dialogues = entry.lines;
                return;
            }
        }

        Debug.LogError($"Catégorie de dialogue '{dialogueCategory}' non trouvée !");
    }
}
