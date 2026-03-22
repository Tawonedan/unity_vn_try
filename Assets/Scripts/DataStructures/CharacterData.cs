using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "VN/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Color nameColor = Color.white; // Warna teks saat nama ini muncul
    
    [Header("Sprites/Ekspresi")]
    public Sprite neutral;
    public Sprite happy;
    public Sprite sad;
    public Sprite angry;
}