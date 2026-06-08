using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FirstSkill", story: "Agent picks first skill in hand", category: "Skill", id: "7b9b53126adba3107cdb8dc4bf6b60dc")]
public partial class FirstSkillAction : Action
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

        Skill firstSkill = hand.Hand[0];

        skill.SetCurrentSkill(firstSkill);
        hand.Hand.RemoveAt(0);

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

