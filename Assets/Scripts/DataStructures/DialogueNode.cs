using UnityEngine;
using System.Collections.Generic;

public enum Expression { Neutral, Happy, Sad, Angry }

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "VN/Dialogue Node")]
public class DialogueNode : ScriptableObject 
{   
    [TextArea(3, 10)]
    public string dialogueText;
    public CharacterData character; // Link ke ScriptableObject karakter
    public Expression expression; // Ekspresi wajah karakter

    [Header("Event Settings")]
    public bool triggersNextTime = false; // Jika dicentang, waktu akan berganti setelah dialog ini
    
    // Ini adalah bagian "Linked List"-nya
    public DialogueNode nextNode; 
    
    // Ini untuk percabangan (Choice)
    public List<Choice> choices;
}



[System.Serializable]
public class Choice {
public string choiceText;
    public DialogueNode targetNode; 

    // Tambahkan 3 baris di bawah ini agar error hilang:
    [Header("Efek Stat")]
    public int healthChange;
    public int moodChange;
    public int witChange;

    [Header("Event Settings")]
    public bool triggersNextTime = false; // Jika dicentang, waktu akan berganti setelah dialog ini
}