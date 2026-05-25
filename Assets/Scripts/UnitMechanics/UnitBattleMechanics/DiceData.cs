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
