using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
public class DiceDisplay : MonoBehaviour
{
    public Dice DiceData;

    public TMP_Text Name;
    public TMP_Text Values;
    public TMP_Text Description;

    public Image Type;

    public Sprite defence;
    public Sprite evade;
    public Sprite slash;
    public Sprite pierce;
    public Sprite blunt;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCardDiaplay();
    }

    public void UpdateCardDiaplay()
    {
        Name.text = DiceData.Name;
        Values.text = DiceData.MinValue.ToString() + " - " + DiceData.MaxValue.ToString();

        if (DiceData.ApliedStatusEffect != null && DiceData.CountAplied > 0)
        {
            Description.text = "Inflict " + DiceData.CountAplied +  " " + DiceData.ApliedStatusEffect.Name;
        }
        else
        {
            Description.text = "";
        }

        if (DiceData.Type == DiceType.Healing || DiceData.Type == DiceType.Defence)
        {
            Type.sprite = defence;
        }
        if (DiceData.Type == DiceType.Evasion)
        {
            Type.sprite = evade;
        }
        if (DiceData.Type == DiceType.SlashDamage)
        {
            Type.sprite = slash;
        }
        if (DiceData.Type == DiceType.PierceDamage)
        {
            Type.sprite = pierce;
        }
        if (DiceData.Type == DiceType.BluntDamage)
        {
            Type.sprite = blunt;
        }
    }
}
*/