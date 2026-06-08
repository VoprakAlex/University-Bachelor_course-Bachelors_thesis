using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpecificTarget", story: "Agent targets [Target] and selects in [SelectedTarget]", category: "Target", id: "3282096cf1c9763ebc0e979ea1d3c8c9")]
public partial class SpecificTargetAction : Action
{
    private TargetComponent _targetComponent;
    private GameManager _gameManager;

    [SerializeReference]
    public BlackboardVariable<GameObject> Target;

    [SerializeReference]
    public BlackboardVariable<GameObject> SelectedTarget;

    protected override Status OnStart()
    {
        _targetComponent = GameObject.GetComponent<TargetComponent>();
        _gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();

        if (_targetComponent == null || _gameManager == null)
            return Status.Failure;

        GameObject targetCharacter = Target?.Value;

        // Пытаемся найти указанную цель среди живых
        if (targetCharacter != null && IsAliveTarget(targetCharacter))
        {
            ActionComponent[] actions =
                targetCharacter.GetComponentsInChildren<ActionComponent>();

            if (actions.Length > 0)
            {
                ActionComponent selectedAction =
                    actions[UnityEngine.Random.Range(0, actions.Length)];

                _targetComponent.SetMainTarget(selectedAction.gameObject);

                if (SelectedTarget != null)
                    SelectedTarget.Value = selectedAction.gameObject;

                return Status.Success;
            }
        }

        // Если цель не найдена — выбираем случайную по фракциям
        return SelectRandomFactionTarget();
    }

    private bool IsAliveTarget(GameObject target)
    {
        return _gameManager.PlayerAlive.Contains(target)
            || _gameManager.AllyAlive.Contains(target)
            || _gameManager.AllyImportantAlive.Contains(target)
            || _gameManager.EnemyAlive.Contains(target)
            || _gameManager.EnemyBossAlive.Contains(target);
    }

    private Status SelectRandomFactionTarget()
    {
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
                return Status.Success;
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

