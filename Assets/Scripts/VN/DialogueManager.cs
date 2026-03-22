using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections; // Wajib untuk Coroutine

public partial class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public GameObject choiceRoot; // Folder 'OptionDialogue' kamu
    public GameObject choicePrefab; // Kita akan bahas ini nanti
    public DialogueNode startNode;

    private DialogueNode currentNode;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [Header("Time & BG Settings")]
    public Image backgroundImage;
    public Sprite roomDay;
    public Sprite roomNight;

    void Start()
    {
    
    if(startNode != null) DisplayNode(startNode);
    
    }

    // Fungsi utama untuk memulai/menampilkan node
    public void DisplayNode(DialogueNode node)
    {
        currentNode = node;

        // 1. Update Teks & Nama
        dialogueText.text = node.dialogueText;
        if (node.character != null)
        {
            nameText.text = node.character.characterName;
            nameText.color = node.character.nameColor;
            
            switch (node.expression)
            {
                case Expression.Happy: characterImage.sprite = node.character.happy; break;
                case Expression.Sad: characterImage.sprite = node.character.sad; break;
                case Expression.Angry: characterImage.sprite = node.character.angry; break;
                default: characterImage.sprite = node.character.neutral; break;
            }
        }
        else
        {
            // Jika tidak ada karakter (narasi saja), sembunyikan gambar & nama
            characterImage.gameObject.SetActive(false);
            nameText.text = ""; 
        }

        // Berhenti dulu kalau ada typing yang sedang jalan (biar gak tumpang tindih)
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        
        // Update Nama & Sprite (Logika Switch-Case dari Objective sebelumnya)
        UpdateUIElements(node);

        // Mulai efek mengetik
        typingCoroutine = StartCoroutine(TypeText(node.dialogueText));

        // 3. Handle Choices
        HandleChoices(node);
        
        IEnumerator TypeText(string text)
        {
            dialogueText.text = "";
            isTyping = true;
            
            foreach (char letter in text.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        isTyping = false;
        }
    }

    private void HandleChoices(DialogueNode node)
    {
        // 1. Bersihkan tombol lama (Clear previous children)
        foreach (Transform child in choiceRoot.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Cek apakah ada pilihan?
        if (node.choices != null && node.choices.Count > 0)
        {
            choiceRoot.SetActive(true);
            foreach (var choice in node.choices)
            {
                // 3. Spawn tombol dari Prefab (Seperti render list)
                GameObject btnObj = Instantiate(choicePrefab, choiceRoot.transform);
                
                // 4. Set teks tombolnya
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

                // 5. Tambahkan listener Klik (onClick)
                btnObj.GetComponent<Button>().onClick.AddListener(() => {
                    SelectChoice(choice);
                });
            }
        }
        else
        {
            choiceRoot.SetActive(false);
        }
    }

    private void SelectChoice(Choice choice)
    {
        // Update Stat lewat StatManager
        StatManager.instance.ChangeStat(choice.moodChange, choice.healthChange, choice.witChange);
        
        // Pindah ke node selanjutnya
        if (choice.targetNode != null)
        {
            DisplayNode(choice.targetNode);
        }

        if (currentNode.triggersNextTime)
        {
            StatManager.instance.NextTime(); // Fungsi ini sudah kita buat di Objective 2
            UpdateEnvironment();
        }
    }

    public void OnClickContinue()
    {
        // Kalau masih ngetik, langsung munculin semua teks (skip typing)
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentNode.dialogueText;
            isTyping = false;
            return;
        }

        // Kalau sudah beres ngetik dan tidak ada pilihan, lanjut ke node berikutnya
        if (currentNode.choices.Count == 0 && currentNode.nextNode != null)
        {
            DisplayNode(currentNode.nextNode);
        }

        if (currentNode.triggersNextTime)
        {
            StatManager.instance.NextTime(); // Fungsi ini sudah kita buat di Objective 2
            UpdateEnvironment();
        }
    }

    private void UpdateUIElements(DialogueNode node)
    {
        if (node.character != null)
        {
            characterImage.gameObject.SetActive(true);
            nameText.text = node.character.characterName;
            nameText.color = node.character.nameColor;

            // Memilih sprite berdasarkan expression yang dipilih di Inspector
            switch (node.expression)
            {
                case Expression.Happy: characterImage.sprite = node.character.happy; break;
                case Expression.Sad: characterImage.sprite = node.character.sad; break;
                case Expression.Angry: characterImage.sprite = node.character.angry; break;
                default: characterImage.sprite = node.character.neutral; break;
            }
        }
        else
        {
            characterImage.gameObject.SetActive(false);
            nameText.text = "Narasi"; // Atau kosongkan
        }
    }

    public void UpdateEnvironment()
{
    if (StatManager.instance.isNight)
    {
        backgroundImage.sprite = roomNight;
        // Kasih tint biru sedikit biar lebih 'malam'
        backgroundImage.color = new Color(0.7f, 0.7f, 0.9f);
    }
    else
    {
        backgroundImage.sprite = roomDay;
        backgroundImage.color = Color.white;
    }
}
}