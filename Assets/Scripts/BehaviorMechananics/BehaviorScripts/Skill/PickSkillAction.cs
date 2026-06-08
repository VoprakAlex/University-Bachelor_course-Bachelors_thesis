using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PickSkill", story: "Agent picks [SkillData]", category: "Skill", id: "84412704f2848676b637383604e8dc93")]
public partial class PickSkillAction : Action
{
    [SerializeReference]
    public BlackboardVariable<SkillData> SkillData;

    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null)
            return Status.Failure;

        if (SkillData?.Value == null)
            return Status.Failure;

        Skill skill = new Skill(SkillData.Value);

        skillComponent.SetCurrentSkill(skill);

        if (hand.Hand.Count > 0)
        {
            hand.Hand.RemoveAt(0);
        }

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

