using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastHpTarget", story: "Agent targets lowest HP [SelectedTarget]", category: "Target", id: "297826749420d16956add584f59e2652")]
public partial class LeastHpTargetAction : Action
{
    private TargetComponent _targetComponent;
    private GameManager _gameManager;

    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;

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
                return Status.Success;
        }

        GameObject selectedCharacter = null;
        int bestHp = int.MaxValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent == null)
                continue;

            int currentHp = healthComponent.CurrentHealth;
            if (selectedCharacter == null || currentHp < bestHp)
            {
                bestHp = currentHp;
                selectedCharacter = target;
            }
        }

        if (selectedCharacter == null)
            return Status.Failure;

        ActionComponent[] actions = selectedCharacter.GetComponentsInChildren<ActionComponent>();
        if (actions.Length == 0)
            return Status.Failure;

        ActionComponent selectedAction = actions[UnityEngine.Random.Range(0, actions.Length)];
        _targetComponent.SetMainTarget(selectedAction.gameObject);

        if (SelectedTarget != null)
            SelectedTarget.Value = selectedAction.gameObject;

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

