using UnityEngine;

/*
[CreateAssetMenu(fileName = "New StatusEffect", menuName = "StatusEffect")]
public class StatusEffect : ScriptableObject
{
    public string Name;

    public string Description;
    

    public float SlachDamageMultiplier;
    public float PierceDamageMultiplier;
    public float BluntDamageMultiplier;
    public float SanityDamageMultiplier;


    public float TrueDamageMultiplierHp;
    public float TrueDamageMultiplierSp;

    public float TrueHealingMultiplierHp;
    public float TrueHealingMultiplierSp;

    public bool InfllictOnHit = false;
    public bool InfllictOnTurnEnd = false;

    public bool LoseNextTurn = false;

   
    public void ApplyEffect(Character target, int Count)
    {
        if (LoseNextTurn) 
        {
            target.LoseNextTurn = 1;
        }


        if (Count >= 0) 
        {
            target.CurrentSlachDamageMultiplier = target.NormalSlachDamageMultiplier + SlachDamageMultiplier * Count;
            target.CurrentPierceDamageMultiplier = target.NormalPierceDamageMultiplier + PierceDamageMultiplier * Count;
            target.CurrentBluntDamageMultiplier = target.NormalBluntDamageMultiplier + BluntDamageMultiplier * Count;
            target.CurrentSanityDamageMultiplier = target.NormalSanityDamageMultiplier + SanityDamageMultiplier * Count;

            target.TakeDamageHp((int)(TrueDamageMultiplierHp * Count));

            target.TakeDamageSp((int)(TrueDamageMultiplierSp * Count));

            target.Heal((int)(TrueHealingMultiplierHp * Count), (int)(TrueHealingMultiplierSp * Count));

        }
    }

    public void OnHit(Character target, int Count)
    {
        if (InfllictOnHit)
        {
            ApplyEffect(target, Count);
        }
        
    }

    public void OnTurnEnd(Character target, int Count)
    {
        if (InfllictOnTurnEnd)
        {
            ApplyEffect(target, Count);
        }

    }
}
*/