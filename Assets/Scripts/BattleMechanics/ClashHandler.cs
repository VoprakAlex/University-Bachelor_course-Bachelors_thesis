using UnityEngine;

public class ClashHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public GameObject Attacker;
    [SerializeField] public GameObject Target;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        TargetComponent targetComponent = attacker.GetComponent<TargetComponent>();

        Target = targetComponent.MainTarget;
        Debug.Log("-------");
        Debug.Log(attacker);
        Debug.Log(targetComponent.MainTarget);
    }

    public bool ShouldRemove(Dice evade, Dice attack)
    {
        if (evade.Type == DiceType.Evade && attack.Type == DiceType.Attack)
        {
            return evade.RolledValue < attack.RolledValue;
        }

        return true;
    }

    public void ExecuteClashSkill(Skill attackerSkill, Skill targetSkill)
    {
        

        while (attackerSkill.Dice.Count > 0 && targetSkill.Dice.Count > 0)
        {
            Dice attackerDice = attackerSkill.Dice[0];
            Dice targetDice = targetSkill.Dice[0];

            Debug.Log("-------");

            Debug.Log(attackerSkill.DamageAffinity);
            Debug.Log(targetSkill.DamageAffinity);

            ExecuteClashDice(attackerDice, targetDice, attackerSkill.DamageAffinity, targetSkill.DamageAffinity);

            if (ShouldRemove(attackerDice, targetDice))
            {
                attackerSkill.Dice.RemoveAt(0);
            }

            if (ShouldRemove(targetDice, attackerDice))
            {
                targetSkill.Dice.RemoveAt(0);
            }
        }

        StatsComponent attackerStats = Attacker.GetComponent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponent<StatsComponent>();

        if (attackerSkill.Dice.Count > 0)
        {
            ExecuteUnopposedSkill(attackerSkill, attackerStats, targetStats);
        }

        if (targetSkill.Dice.Count > 0)
        {
            ExecuteUnopposedSkill(targetSkill, targetStats, attackerStats);
        }
    }

    public void ExecuteClashDice(Dice attackerDice, Dice targetDice, DamageAffinity attackerAffinity, DamageAffinity targetAffinity)
    {
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        int attackerRoll = attackerDice.Roll();
        int targetRoll = targetDice.Roll();
        Debug.Log("-------");
        Debug.Log(attackerDice.Type);
        Debug.Log(attackerDice.DamageType);

        Debug.Log(attackerAffinity);
        Debug.Log(attackerRoll);
        Debug.Log(targetDice.Type);
        Debug.Log(targetDice.DamageType);

        Debug.Log(targetAffinity);
        Debug.Log(targetRoll);



        // ATTACK vs ATTACK
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll > targetRoll)
                targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            else if (targetRoll > attackerRoll)
                attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);

            return;
        }

        // DEFEND vs DEFEND
        if (attackerDice.Type == DiceType.Defend && targetDice.Type == DiceType.Defend)
        {
            attackerStats.IncreaseShield(attackerRoll);
            targetStats.IncreaseShield(targetRoll);
            return;
        }

        // ATTACK vs DEFEND
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Defend)
        {
            if (targetRoll >= attackerRoll)
            {
                return;
            }

            targetStats.IncreaseShield(targetRoll);
            attackerStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Defend && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll >= targetRoll)
            {
                return;
            }

            attackerStats.IncreaseShield(attackerRoll);
            targetStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }


        // ATTACK vs EVADE
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Evade)
        {
            if (targetRoll >= attackerRoll)
                return;

            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Evade && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll >= targetRoll)
                return;

            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }

        // ATTACK vs COUNTER
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Counter)
        {
            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Counter && targetDice.Type == DiceType.Attack)
        {
            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity, StatsComponent attackerStats, StatsComponent targetStats)
    {
        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage(rolledValue, dice.DamageType, affinity);

                break;

            default:
                break;
        }
    }

    public void ExecuteUnopposedSkill(Skill skill, StatsComponent attackerStats, StatsComponent targetStats)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            ExecuteUnopposedDice(currentDice, skill.DamageAffinity, attackerStats, targetStats);

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteClash(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent attackerSkillComponent = Attacker.GetComponent<SkillComponent>();
        SkillComponent targetSkillComponent = Target.GetComponent<SkillComponent>();

        ExecuteClashSkill(attackerSkillComponent.CurrentSkill, targetSkillComponent.CurrentSkill);

        Attacker = null;
        Target = null;

    }
}