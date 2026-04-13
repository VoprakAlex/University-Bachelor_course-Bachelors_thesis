using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool Clashable { get; private set; }
    public DamageAffinity DamageAffinity { get; private set; }

    public List<Dice> Dice { get; private set; }

    public Skill(SkillData source)
    {
        Name = source.Name;
        Description = source.Description;
        Clashable = source.Clashabale;
        DamageAffinity = source.DamageAffinity;
        Dice = source.Dice.Select(d => new Dice(d)).ToList();
    }
}
