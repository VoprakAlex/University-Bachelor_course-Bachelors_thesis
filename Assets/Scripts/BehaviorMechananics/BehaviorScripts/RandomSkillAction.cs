using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomSkill", story: "Agent picks random skill", category: "Action", id: "31eb3b8db8a4def2861a69b7dcc92b64")]
public partial class RandomSkillAction : Action
{
    [Header("Components")]
    private HandComponent _handComponent;
    private SkillComponent _skillComponent;

    protected override Status OnStart()
    {
        _handComponent = GameObject.GetComponent<HandComponent>();
        _skillComponent = GameObject.GetComponent<SkillComponent>();

        int randomIndex = UnityEngine.Random.Range(0, _handComponent.Hand.Count);

        Skill randomSkill = _handComponent.Hand[randomIndex];

        _skillComponent.SetCurrentSkill(randomSkill);

        _handComponent.Hand.RemoveAt(randomIndex);

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

