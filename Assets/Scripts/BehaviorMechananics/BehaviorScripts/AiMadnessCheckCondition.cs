using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "AIMadnessCheck ", story: "If [GameObject] is mad", category: "Conditions", id: "47fb75d3b4ba036f11aae2782d72cf42")]
public partial class AiMadnessCheckCondition : Condition
{
    private SanityComponent _sanityComponent;

    public override bool IsTrue()
    {
        if (GameObject == null)
            return false;

        if (_sanityComponent == null)
            _sanityComponent = GameObject.GetComponentInParent<SanityComponent>();

        return _sanityComponent.IsMad;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
