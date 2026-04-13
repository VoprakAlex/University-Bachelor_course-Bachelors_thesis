using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    public string Name;

    public int MaxHP;
    public int MaxSP;

    public int StartingShield;

    public float MinSpeed;
    public float MaxSpeed;

    public float StaggerPercent;

    public DamageType StandardDamageType = DamageType.Slashing;

    public int BaseAttackPower;

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
        { DamageAffinity.Ruby, ResistanceTier.Normal },
        { DamageAffinity.Sapphire, ResistanceTier.Normal },
        { DamageAffinity.Topaz, ResistanceTier.Normal },
        { DamageAffinity.Garnet, ResistanceTier.Normal },
        { DamageAffinity.Amethyst, ResistanceTier.Normal },
        { DamageAffinity.Diamond, ResistanceTier.Normal }
    };
}
