using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomTarget", story: "Targets random enemy [SelectedTarget]", category: "Action", id: "601d1d600c3afd7b3217c617f71fe010")]
public partial class RandomTargetAction : Action
{
    private TargetComponent _targetComponent;
    private GameManager _gameManager;

    [SerializeReference]
    public BlackboardVariable<GameObject> SelectedTarget;

    protected override Status OnStart()
    {
        _targetComponent = GameObject.GetComponent<TargetComponent>();
        _gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();

        if (_targetComponent == null || _gameManager == null)
            return Status.Failure;

        GameObject ownerCharacter = GameObject.transform.root.gameObject;

        List<GameObject> possibleTargets = new();

        switch (ownerCharacter.tag)
        {
            case GameManager.EnemyTag:
            case GameManager.EnemyBossTag:
                possibleTargets.AddRange(_gameManager.PlayerAlive);
                possibleTargets.AddRange(_gameManager.AllyAlive);
                possibleTargets.AddRange(_gameManager.AllyImportantAlive);
                break;

            case GameManager.AllyTag:
            case GameManager.AllyImportantTag:
                possibleTargets.AddRange(_gameManager.EnemyAlive);
                possibleTargets.AddRange(_gameManager.EnemyBossAlive);
                break;

            case GameManager.PlayerTag:
                return Status.Failure;
        }

        if (possibleTargets.Count == 0)
            return Status.Failure;

        GameObject selectedCharacter =
            possibleTargets[UnityEngine.Random.Range(0, possibleTargets.Count)];

        ActionComponent[] actions =
            selectedCharacter.GetComponentsInChildren<ActionComponent>();

        if (actions.Length == 0)
            return Status.Failure;

        ActionComponent selectedAction =
            actions[UnityEngine.Random.Range(0, actions.Length)];

        _targetComponent.SetMainTarget(selectedAction.gameObject);

        if (SelectedTarget != null)
            SelectedTarget.Value = selectedAction.gameObject;

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

