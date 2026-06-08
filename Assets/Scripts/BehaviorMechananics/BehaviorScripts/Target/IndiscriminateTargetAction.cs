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

        if (_targetComponent == null || _gameManager == null)
            return Status.Failure;

        List<GameObject> possibleTargets = new();

        possibleTargets.AddRange(_gameManager.PlayerAlive);
        possibleTargets.AddRange(_gameManager.AllyAlive);
        possibleTargets.AddRange(_gameManager.AllyImportantAlive);
        possibleTargets.AddRange(_gameManager.EnemyAlive);
        possibleTargets.AddRange(_gameManager.EnemyBossAlive);

        // Удаляем владельца текущего Action
        GameObject ownerCharacter = GameObject.transform.root.gameObject;

        possibleTargets.RemoveAll(target => target == ownerCharacter);

        if (possibleTargets.Count == 0)
            return Status.Failure;

        // Выбираем случайного персонажа
        GameObject selectedCharacter =
            possibleTargets[UnityEngine.Random.Range(0, possibleTargets.Count)];

        // Получаем все Action персонажа
        ActionComponent[] actions =
            selectedCharacter.GetComponentsInChildren<ActionComponent>();

        if (actions.Length == 0)
            return Status.Failure;

        // Выбираем случайный Action
        ActionComponent selectedAction =
            actions[UnityEngine.Random.Range(0, actions.Length)];

        _targetComponent.SetMainTarget(selectedAction.gameObject);

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

