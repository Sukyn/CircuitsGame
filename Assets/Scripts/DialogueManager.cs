using UnityEngine;
using TMPro;
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
    public List<DialogueEntry> dialogues;  // Liste des dialogues au lieu d'un Dictionary
}

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBubble;
    public TextMeshProUGUI dialogueText;
    private bool isTalking = false;
    private List<string> dialogues = new List<string>();
    private int currentDialogueIndex = 0;
    public string dialogueSection = "intro";  // Valeur par défaut

    void Start()
    {
        LoadDialogues(dialogueSection);
        if (dialogues.Count > 0)
        {
            ShowDialogue(dialogues[currentDialogueIndex]);
        }
    }

    void Update()
    {
        if (isTalking && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            NextDialogue();
        }
    }

    public void ShowDialogue(string text)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        dialogueText.text = text;
        isTalking = true;
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex >= dialogues.Count)
        {
            CloseDialogue();
        }
        else
        {
            ShowDialogue(dialogues[currentDialogueIndex]);
        }
    }

    private void CloseDialogue()
    {
        dialogueText.transform.parent.gameObject.SetActive(false);
        isTalking = false;
    }

    private void LoadDialogues(string dialogueCategory)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "dialogues.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Fichier dialogues.json introuvable !");
            return;
        }

        string jsonContent = File.ReadAllText(filePath);
        DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);

        if (dialogueData == null || dialogueData.dialogues == null)
        {
            Debug.LogError("Erreur lors du chargement des dialogues !");
            return;
        }

        foreach (var entry in dialogueData.dialogues)
        {
            if (entry.category == dialogueCategory)
            {
                dialogues = entry.lines;
                return;
            }
        }

        Debug.LogError("Catégorie de dialogue non trouvée !");
    }
}
