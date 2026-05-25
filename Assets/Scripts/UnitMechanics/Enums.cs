using UnityEngine;


public enum DamageType
{
    Slashing = 0,
    Piercing = 1,
    Bludgening = 2,
}

public enum DamageAffinity
{
    Physical = 0,   // #B0B0B0 (серый)
    Tremor = 1,     // #8B5A2B (землистый коричневый)
    Poison = 2,     // #4CAF50 (ядовито-зелёный)
    Bleed = 3,      // #B71C1C (тёмно-кроваво-красный)
    Electric = 4,   // #00E5FF (яркий неон-циан)
    Fire = 5,       // #FF3D00 (огненно-оранжево-красный)
    Cold = 6,       // #4FC3F7 (холодный голубой)
    Mind = 7,       // #9C27B0 (фиолетовый)
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
}