using System.Xml.Linq;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiceView : MonoBehaviour
{
    [Header("Data")]
    public Dice Data;

    [Header("Values")]
    public TMP_Text MinValue;
    public TMP_Text MaxValue;

    [Header("Image")]
    public Image DiceTypeImage;

    [Header("Sprites")]
    public Sprite Defend;
    public Sprite Evade;
    public Sprite Counter;

    public Sprite Slashing;
    public Sprite Piercing;
    public Sprite Bludgening;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDiceView();
    }

    public void SetDice(Dice dice)
    {
        Data = dice;
        UpdateDiceView();
    }

    public void UpdateDiceView()
    {
        MinValue.text = Data.MinValue.ToString();
        MaxValue.text = Data.MaxValue.ToString();

        switch (Data.Type)
        {
            case DiceType.Attack:
                switch (Data.DamageType)
                {
                    case DamageType.Slashing:
                        DiceTypeImage.sprite = Slashing;
                        break;

                    case DamageType.Piercing:
                        DiceTypeImage.sprite = Piercing;
                        break;

                    case DamageType.Bludgening:
                        DiceTypeImage.sprite = Bludgening;
                        break;
                }
                break;

            case DiceType.Defend:
                DiceTypeImage.sprite = Defend;
                break;

            case DiceType.Evade:
                DiceTypeImage.sprite = Evade;
                break;

            case DiceType.Counter:
                DiceTypeImage.sprite = Counter;
                break;
        } 
    }
}