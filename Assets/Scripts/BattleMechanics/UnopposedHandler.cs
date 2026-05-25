using UnityEngine;

public class UnopposedHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject Attacker;
    [SerializeField] private GameObject Target;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        Target = attacker.GetComponent<TargetComponent>().MainTarget;
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity)
    {
        StatsComponent attackerStats = Attacker.GetComponent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponent<StatsComponent>();

        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage( rolledValue, dice.DamageType, affinity );

                break;

            //case DiceType.Defend:

                //attackerStats.IncreaseShield(rolledValue);

                //break;

            default:
                break;
        }
    }

    public void ExecuteUnopposedSkill(Skill skill)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            ExecuteUnopposedDice(currentDice, skill.DamageAffinity);

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteUnopposed(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent skillComponent = Attacker.GetComponent<SkillComponent>();

        ExecuteUnopposedSkill(skillComponent.CurrentSkill);
    }
}