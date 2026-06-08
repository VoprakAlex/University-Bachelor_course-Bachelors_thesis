using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [Header("Data")]
    public Skill Data;

    [Header("Images")]
    public Image CardBackground;
    public TMP_Text NameText;
    public TMP_Text StatusText;

    [Header("Dices")]
    public Transform DiceContainer;
    public DiceView DicePrefab;
    public List<DiceView> Dices = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillView();
    }

    public void SetSkill(Skill skill)
    {
        Data = skill;
        UpdateSkillView();
    }

    public void UpdateSkillView()
    {
        NameText.text = Data.Name;

        if (!Data.Clashable)
        {
            StatusText.gameObject.SetActive(true);
            StatusText.text = "Unclashable";
        }
        else
        {
            StatusText.gameObject.SetActive(false);
        }

        CardBackground.color = GetAffinityColor(Data.DamageAffinity);

        ClearDice();
        SpawnDice();
    }

    private void SpawnDice()
    {
        foreach (var dice in Data.Dice)
        {
            var view = Instantiate(DicePrefab, DiceContainer);
            view.SetDice(dice);
            Dices.Add(view);
        }
    }

    private void ClearDice()
    {
        foreach (var dice in Dices)
        {
            if (dice != null)
                Destroy(dice.gameObject);
        }

        Dices.Clear();
    }

    private Color GetAffinityColor(DamageAffinity affinity)
    {
        return affinity switch
        {
            DamageAffinity.Physical => new Color(0.69f, 0.69f, 0.69f),   // #B0B0B0
            DamageAffinity.Tremor => new Color(0.545f, 0.353f, 0.169f), // #8B5A2B
            DamageAffinity.Poison => new Color(0.298f, 0.737f, 0.318f),  // #4CAF50
            DamageAffinity.Bleed => new Color(0.717f, 0.109f, 0.109f),  // #B71C1C
            DamageAffinity.Electric => new Color(0f, 0.898f, 1f),        // #00E5FF
            DamageAffinity.Fire => new Color(1f, 0.239f, 0f),            // #FF3D00
            DamageAffinity.Cold => new Color(0.31f, 0.765f, 0.949f),     // #4FC3F7
            DamageAffinity.Mind => new Color(0.61f, 0.156f, 0.705f),     // #9C27B0
            _ => Color.white
        };
    }
}