using UnityEngine;
using UnityEngine.UI; // Tambahkan ini

public class StatManager : MonoBehaviour
{
    public static StatManager instance;

    [Header("Stats")]
    public int mood = 50;
    public int health = 100;
    public int wit = 10;

    public int maxStat = 100; // Standar maksimal untuk bar

    [Header("UI References")]
    public Slider healthBar;
    public Slider witBar;
    public Slider moodBar;
    
    [Header("Time System")]
    public int day = 1;
    public bool isNight = false;

    private void Awake()
    {
        // Singleton pattern: Memastikan hanya ada 1 StatManager
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start() { UpdateUI(); }

    public void ChangeStat(int m, int h, int w)
    {
        // Mathf.Clamp memastikan angka tidak kurang dari 0 atau lebih dari 100
        mood = Mathf.Clamp(mood + m, 0, 100);
        health = Mathf.Clamp(health + h, 0, 100);
        wit = Mathf.Clamp(wit + w, 0, 100);

        UpdateUI();
    }

    public void UpdateUI()
    {
        // Slider.value butuh 0 sampai 1, jadi kita bagi 100
        if (moodBar) moodBar.value = mood / 100f;
        if (healthBar) healthBar.value = health / 100f;
        if (witBar) witBar.value = wit / 100f;
        
        Debug.Log($"Stats: M:{mood} H:{health} W:{wit}");
    }

    public void NextTime()
    {
        if (isNight)
        {
            day++;
            isNight = false;
        }
        else
        {
            isNight = true;
        }
        Debug.Log($"Waktu berganti: Day {day} - {(isNight ? "Malam" : "Siang")}");
    }
}