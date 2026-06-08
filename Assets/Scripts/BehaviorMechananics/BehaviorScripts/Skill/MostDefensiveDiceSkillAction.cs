using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostDefensiveDiceSkill", story: "Agent picks skill with most defensive dice", category: "Skill", id: "0d0fa7cf46d3beed9b27c077401ee2a0")]
public partial class MostDefensiveDiceSkillAction : Action
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
            .OrderByDescending(GetDefensiveDiceCount)
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

