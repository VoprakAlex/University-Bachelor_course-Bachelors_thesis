using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomSkill", story: "Agent picks random skill", category: "Skill", id: "31eb3b8db8a4def2861a69b7dcc92b64")]
public partial class RandomSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skill = action.GetComponent<SkillComponent>();

        if (hand == null || skill == null || hand.Hand.Count == 0)
            return Status.Failure;

        int randomIndex = UnityEngine.Random.Range(0, hand.Hand.Count);
        Skill randomSkill = hand.Hand[randomIndex];

        skill.SetCurrentSkill(randomSkill);
        hand.Hand.RemoveAt(randomIndex);

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

