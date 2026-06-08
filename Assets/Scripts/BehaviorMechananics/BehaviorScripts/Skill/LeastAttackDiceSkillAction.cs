using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastAttackDiceSkill", story: "Agent picks skill with least Attack dice", category: "Skill", id: "c5702200b60fa757ff8409174d782ef8")]
public partial class LeastAttackDiceSkillAction : Action
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
            .OrderBy(skill =>
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

