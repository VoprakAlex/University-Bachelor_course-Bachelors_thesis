using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Scriptable Objects/Dice")]
public class DiceData : ScriptableObject
{
    public string Name;
    public string Description;

    public int MinValue;
    public int MaxValue;

    public DiceType Type;
    public DiceTargetType TargetType;

    public DamageType Damage;

    public int ThrowDice()
    {
        return Random.Range(MinValue, MaxValue);
    }
}
