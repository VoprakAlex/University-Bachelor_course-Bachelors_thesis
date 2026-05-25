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
