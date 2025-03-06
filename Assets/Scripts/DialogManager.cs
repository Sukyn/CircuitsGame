using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class DialogEntry {
    public string speaker;
    public string team;
    public List<string> lines;
}

public class DialogManager : MonoBehaviour {
    public GameObject dialogCanvas;
    public TextMeshProUGUI dialogText;
    public Image speakerBannerImage;
    public AudioSource audioSource;
    private bool isTyping = false;
    private List<string> lines = new List<string>();
    private string headerName;
    private int currentLineIndex;
    public TextAsset jsonFile;
    public float typingSpeed = 0.02f;


    void Start() {
        LoadDialogs();

        if (dialogCanvas.activeSelf) {
            StartDialog();
        }
    }

    private void LoadDialogs() {

        string jsonContent = jsonFile.text;
        DialogEntry dialogData = JsonUtility.FromJson<DialogEntry>(jsonContent);

        if (dialogData == null) {
            Debug.LogError("Error while loading dialogs !");
            return;
        }

        lines = dialogData.lines;
        headerName = dialogData.speaker + dialogData.team; 
    }

    private IEnumerator ShowDialog(string text) {
        isTyping = true;
        dialogText.text = "";

        ApplyBannerFromSpeakerAndTeam();

        audioSource.Play();

        foreach (char letter in text) {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        StopTyping();
    }

    private void ApplyBannerFromSpeakerAndTeam() {
        string bannerPath = "Sprites/Headers/" + headerName;

        Sprite speakerBanner = Resources.Load<Sprite>(bannerPath);

        if (speakerBanner != null) {
            speakerBannerImage.sprite = speakerBanner;
        } else {
            Debug.LogError($"Sprite {bannerPath} not found");
        }
    }

    private void NextDialog() {
        currentLineIndex++;

        if (ValidIndex()) {
            StartCoroutine(ShowDialog(lines[currentLineIndex]));
        } else {
            CloseModal();
        }

    }

    private void CloseModal() {
        dialogCanvas.SetActive(false);
        ResetDialog();
    }


    private void StartDialog() {
        ResetDialog();
        NextDialog();
    }

    private void StopTyping() {
        StopAllCoroutines();
        audioSource.Stop();
        isTyping = false;
        dialogText.text = ValidIndex() ? lines[currentLineIndex] : "";
    }
    private void ResetDialog() {
        currentLineIndex = -1;
        StopTyping();
    }

    private bool ValidIndex() {
        return (currentLineIndex >= 0 && currentLineIndex < lines.Count);
    }

    public void OnDialogButtonPressed() {
        if (isTyping) {
            StopTyping();
        } else {
            NextDialog();
        }
    }

    public void ShowModal() {
        if(lines.Count == 0) {
            LoadDialogs();
        }

        dialogCanvas.SetActive(true);
        StartDialog();
    }
}
