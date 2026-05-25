using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IndiscriminateTarget", story: "Targets random unit [SelectedTarget]", category: "Action", id: "79316144a327f8967d511cbe7545e330")]
public partial class IndiscriminateTargetAction : Action
{
    [Header("Components")]
    private TargetComponent _targetComponent;
    private GameManager _gameManager;

    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;

    protected override Status OnStart()
    {
        _targetComponent = GameObject.GetComponent<TargetComponent>();
        _gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();

        List<GameObject> possibleTargets = new();

        possibleTargets.AddRange(_gameManager.PlayerAlive);
        possibleTargets.AddRange(_gameManager.AllyAlive);
        possibleTargets.AddRange(_gameManager.AllyImportantAlive);
        possibleTargets.AddRange(_gameManager.EnemyAlive);
        possibleTargets.AddRange(_gameManager.EnemyBossAlive);

        possibleTargets.Remove(GameObject);

        if (possibleTargets.Count == 0)
        {
            Debug.Log("No valid targets found");
            return Status.Failure;
        }

        GameObject selected = possibleTargets[UnityEngine.Random.Range(0, possibleTargets.Count)];

        _targetComponent.SetMainTarget(selected);

        SelectedTarget.Value = selected;

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

using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StartRound")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StartRound", message: "Target", category: "Events", id: "36052193b762e1ee0dd36ea8c70a11cc")]
public sealed partial class StartRound : EventChannel { }