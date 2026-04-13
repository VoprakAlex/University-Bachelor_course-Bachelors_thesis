using UnityEngine;

public class Dice : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public int MinValue { get; private set; }
    public int MaxValue { get; private set; }

    public DiceType Type { get; private set; }
    public DiceTargetType TargetType { get; private set; }
    public DamageType DamageType { get; private set; }

    public int RolledValue { get; private set; }
    public bool WasUsedInClash { get; set; }

    public Dice(DiceData source)
    {
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

