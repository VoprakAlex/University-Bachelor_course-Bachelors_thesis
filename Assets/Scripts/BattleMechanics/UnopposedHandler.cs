using UnityEngine;
using System.Collections;

public class UnopposedHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject Attacker;
    [SerializeField] private GameObject Target;

    private const float DiceDelay = 0.15f;
    private const float AnimationWait = 1.0f;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        Target = attacker.GetComponent<TargetComponent>().MainTarget;
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity)
    {
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage( rolledValue, dice.DamageType, affinity );

                break;
            default:
                break;
        }
    }

    public IEnumerator ExecuteUnopposedDiceCoroutine(Dice dice, DamageAffinity affinity)
    {
        Debug.Log("Couroutine Start");
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        CharacterAnimatorController attackerAnimator =  Attacker.GetComponentInParent<CharacterAnimatorController>();
        CharacterAnimatorController targetAnimator = Target.GetComponentInParent<CharacterAnimatorController>();

        ValueChange valueChange = Attacker.GetComponentInParent<ValueChange>();

        int rolledValue = dice.Roll();

        yield return new WaitForSeconds(0.1f);

        if (valueChange != null)
            valueChange.SetText(rolledValue.ToString());

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage(rolledValue, dice.DamageType, affinity);

                break;
            default:
                break;
        }

        if (attackerAnimator != null)
            attackerAnimator.PlayAttack();

        if (targetAnimator != null)
            targetAnimator.PlayHit();

        yield return new WaitForSeconds(1.0f);

        if (valueChange != null)
            valueChange.ClearText();

        Debug.Log("Couroutine End");
    }

    public void ExecuteUnopposedSkill(Skill skill)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            StartCoroutine(ExecuteUnopposedDiceCoroutine(currentDice, skill.DamageAffinity));

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteUnopposed(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent skillComponent = Attacker.GetComponent<SkillComponent>();

        ExecuteUnopposedSkill(skillComponent.CurrentSkill);

        Attacker = null;
        Target = null;
    }
}