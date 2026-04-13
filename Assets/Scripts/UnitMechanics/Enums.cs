using UnityEngine;


public enum DamageType
{
    Slashing = 0,
    Piercing = 1,
    Bludgening = 2,
}

public enum DamageAffinity
{
    Ruby = 0,       // HP
    Sapphire = 1,   // SP
    Topaz = 2,      // Stagger
    Garnet = 3,     // HP + Stagger
    Amethyst = 4,   // HP + SP
    Diamond = 5     // все
}

public enum ResistanceTier
{
    Immune = 0,       // multiplier 0.1f 
    Ineffective = 1,  // multiplier 0.5f
    Resilient = 2,    // multiplier 0.75f
    Normal = 3,       // multiplier 1.0f
    Vulnerable = 4,   // multiplier 1.25f
    Effective = 5,    // multiplier 1.5f
    Fatal = 6         // multiplier 2.0f
}

public static class ResistanceData
{
    public static float GetMultiplier(this ResistanceTier tier)
    {
        return tier switch
        {
            ResistanceTier.Immune => 0.1f,
            ResistanceTier.Ineffective => 0.5f,
            ResistanceTier.Resilient => 0.75f,
            ResistanceTier.Normal => 1.0f,
            ResistanceTier.Vulnerable => 1.25f,
            ResistanceTier.Effective => 1.5f,
            ResistanceTier.Fatal => 2.0f,
            _ => 1.0f,
        };
    }
}

public enum DiceType
{
    Attack = 0,
    Defend = 1,
    Evade = 2,
    Counter = 3,
}

public enum DiceTargetType
{
    MainOnly = 0,
    AllTargets = 1,
    FirstSubTarget = 2,
    AllSubTargets = 3,
}

public enum SkillTargetType
{
    Enemies = 0,      
    Allies = 1,     
    Indiscriminate = 2,
    Self = 3,
    IndiscriminateAndSelf = 4,
}