using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayerMadnessCheck ", story: "If [GameObject] is Player, not staggered, and mad, continue.", category: "Flow", id: "023159a14d499ec71de9c230f9ffb874")]
public partial class PlayerMadnessCheckModifier : Modifier
{
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;

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

        if (_sanityComponent == null)
            _sanityComponent = GameObject.GetComponentInParent<SanityComponent>();

        return _staggerComponent != null && _sanityComponent != null;
    }

    private bool CanContinue()
    {
        return GameObject.CompareTag("Player") && !_staggerComponent.IsStaggered && _sanityComponent.IsMad;
    }
}

