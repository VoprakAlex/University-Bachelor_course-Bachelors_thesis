using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AICheck ", story: "If [GameObject] is not Player and not staggered, continue.", category: "Flow", id: "2f2dd20cec6da113a30d926f3232ab3d")]
public partial class AiCheckModifier : Modifier
{
    private StaggerComponent _staggerComponent;

    protected override Status OnStart()
    {
        if (!CacheComponents())
            return Status.Failure;

        if (!CanContinue())
            return Status.Failure;

        if (Child == null)
            return Status.Success;

        Status childStatus = StartNode(Child);
        return childStatus == Status.Running || childStatus == Status.Waiting
            ? Status.Waiting
            : childStatus;
    }

    protected override Status OnUpdate()
    {
        return Child.CurrentStatus;
    }

    protected override void OnEnd()
    {
    }

    private bool CacheComponents()
    {
        if (GameObject == null)
            return false;

        if (_staggerComponent == null)
            _staggerComponent = GameObject.GetComponentInParent<StaggerComponent>();

        return _staggerComponent != null;
    }

    private bool CanContinue()
    {
        return !GameObject.CompareTag("Player") && !_staggerComponent.IsStaggered;
    }
}

