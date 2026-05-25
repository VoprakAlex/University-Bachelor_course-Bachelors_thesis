using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice")]
public class DiceData : ScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;

    [Header("Values")]
    public int MinValue;
    public int MaxValue;

    [Header("Types")]
    public DiceType Type;
    public DiceTargetType TargetType;
    public DamageType Damage;
}

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dice
{
    [Header("Data")]
    [field: SerializeField] public DiceData Data { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [Header("Values")]
    [field: SerializeField] public int MinValue { get; private set; }
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int RolledValue { get; private set; }

    [Header("Types")]
    [field: SerializeField] public DiceType Type { get; private set; }
    [field: SerializeField] public DiceTargetType TargetType { get; private set; }
    [field: SerializeField] public DamageType DamageType { get; private set; }

    [Header("RollEvents")]
    public UnityEvent<int> Rolled;

    public Dice(DiceData source)
    {
        Data = source;
        Name = source.Name;
        Description = source.Description;
        MinValue = source.MinValue;
        MaxValue = source.MaxValue;
        Type = source.Type;
        TargetType = source.TargetType;
        DamageType = source.Damage;
    }

    public int Roll()
    {
        RolledValue = Random.Range(MinValue, MaxValue + 1);
        Rolled.Invoke(RolledValue);
        return RolledValue;
    }
}

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

using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillData : ScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;

    [Header("Skill")]
    public bool Clashabale = true;
    public DamageAffinity DamageAffinity;

    [Header("Dices")]
    public List<DiceData> Dice = new List<DiceData>();
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [Header("Data")]
    [field: SerializeField] public SkillData Data { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [Header("Skill")]
    [field: SerializeField] public bool Clashable { get; private set; }
    [field: SerializeField] public DamageAffinity DamageAffinity { get; private set; }

    [Header("Dices")]
    [field: SerializeField] public List<Dice> Dice { get; private set; }

    public Skill(SkillData source)
    {
        Data = source;
        Name = source.Name;
        Description = source.Description;
        Clashable = source.Clashabale;
        DamageAffinity = source.DamageAffinity;
        Dice = source.Dice.Select(d => new Dice(d)).ToList();
    }
}

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