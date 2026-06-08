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
        return RolledValue;
    }
}

