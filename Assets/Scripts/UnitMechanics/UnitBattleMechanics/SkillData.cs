using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class SkillData : ScriptableObject
{

    public string Name;
    public string Description;

    public bool Clashabale = true;

    public DamageAffinity DamageAffinity;

    public List<DiceData> Dice = new List<DiceData>();

}