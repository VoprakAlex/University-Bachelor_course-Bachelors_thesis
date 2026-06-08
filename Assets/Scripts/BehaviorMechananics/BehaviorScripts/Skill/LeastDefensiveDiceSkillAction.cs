using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastDefensiveDiceSkill", story: "Agent picks skill with least defensive dice", category: "Action", id: "b11c7be355848edd4b1ee7f1dedff38a")]
public partial class LeastDefensiveDiceSkillAction : Action
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
            .OrderBy(GetDefensiveDiceCount)
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

        return Status.Success;
    }

    private int GetDefensiveDiceCount(Skill skill)
    {
        int count = 0;

        foreach (var dice in skill.Dice)
        {
            if (dice.Type == DiceType.Defend ||
                dice.Type == DiceType.Evade ||
                dice.Type == DiceType.Counter)
            {
                count++;
            }
        }

        return count;
    }

    protected override Status OnUpdate() => CurrentStatus;
    protected override void OnEnd() { }
}

