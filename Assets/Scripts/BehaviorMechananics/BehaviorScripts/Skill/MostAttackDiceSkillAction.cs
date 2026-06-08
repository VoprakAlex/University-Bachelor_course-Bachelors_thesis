using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostAttackDiceSkill", story: "Agent picks skill with most Attack dice", category: "Action", id: "b9cf27256e818909165facc0ccc6b35b")]
public partial class MostAttackDiceSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill selectedSkill = hand.Hand
            .OrderByDescending(skill =>
                skill.Dice.Count(d => d.Type == DiceType.Attack))
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return CurrentStatus;
    }

    protected override void OnEnd()
    {
    }
}

