using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    [Header("Name")]
    public string Name;

    [Header("Characteristics")]
    public int MaxHP;
    public int MaxSP;
    public int StartingShield;
    public float StaggerPercent;

    [Header("Speed")]
    public int MinSpeed;
    public int MaxSpeed;

    [Header("Resistances")]
    [SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> StandardDamageTypeResistances = new SerializedDictionary<DamageType, ResistanceTier>
    {
        { DamageType.Slashing, ResistanceTier.Normal },
        { DamageType.Piercing, ResistanceTier.Normal },
        { DamageType.Bludgening, ResistanceTier.Normal }
    };

    [SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> StandardDamageAffinityResistances = new SerializedDictionary<DamageAffinity, ResistanceTier>
    {
        { DamageAffinity.Physical, ResistanceTier.Normal },
        { DamageAffinity.Tremor, ResistanceTier.Normal },
        { DamageAffinity.Poison, ResistanceTier.Normal },
        { DamageAffinity.Bleed, ResistanceTier.Normal },
        { DamageAffinity.Electric, ResistanceTier.Normal },
        { DamageAffinity.Fire, ResistanceTier.Normal },
        { DamageAffinity.Cold, ResistanceTier.Normal  } ,
        { DamageAffinity.Mind, ResistanceTier.Normal  }
    };

    [Header("Skills")]
    [SerializeField] public List<SkillData> Skills = new List<SkillData>();
}
