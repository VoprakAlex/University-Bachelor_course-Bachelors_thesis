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