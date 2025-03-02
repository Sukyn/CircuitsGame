using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class DialogueEntry
{
    public string category;
    public string speaker;
    public string team;
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
    public Image speakerBannerImage;
    public AudioClip typingSound;
    private AudioSource audioSource;
    private bool isTalking = false;
    private bool isTyping = false;
    private List<string> dialogues = new List<string>();
    private string headerName;
    private int currentDialogueIndex = 0;
    public string dialogueSection = "tutoriel_1";
    public float typingSpeed = 0.02f;
    public GameObject level;

    void Start() 
    {
        level?.SetActive(false);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
        }

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
                StopAllCoroutines();
                audioSource.Stop();
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

        // Charge et applique la bannière dynamique en fonction du speaker et team
        ApplyBannerFromSpeakerAndTeam();

        if (typingSound != null && audioSource != null)
        {
            audioSource.clip = typingSound;
            audioSource.Play();
        }

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        audioSource.Stop();
        isTyping = false;
    }

    private void ApplyBannerFromSpeakerAndTeam() {
        string bannerPath = "Sprites/Headers/" + headerName;

        // Recherche du sprite à partir du chemin
        Sprite speakerBanner = Resources.Load<Sprite>(bannerPath);

        if (speakerBanner != null) {
            speakerBannerImage.sprite = speakerBanner;
        } else {
            speakerBannerImage = null;
            Debug.LogError($"Sprite {bannerPath} non trouvé");
        }
    }

    private void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex >= dialogues.Count) {
            CloseDialogue();
        } else {
            StartCoroutine(ShowDialogue(dialogues[currentDialogueIndex]));
        }
    }

    private void CloseDialogue()
    {
        isTalking = false;
        dialogueBackground.transform.parent.gameObject.SetActive(false);
        level?.SetActive(true);
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

        // Recherche de la catégorie de dialogue dans la liste
        DialogueEntry foundDialogue = dialogueData.dialogues.Find(d => d.category == dialogueCategory);

        if (foundDialogue == null)
        {
            Debug.LogError($"Catégorie de dialogue '{dialogueCategory}' non trouvée !");
            return;
        }

        // Si la catégorie est trouvée, affecte les dialogues
        dialogues = foundDialogue.lines;
        headerName = foundDialogue.speaker + foundDialogue.team; 
    }
}
