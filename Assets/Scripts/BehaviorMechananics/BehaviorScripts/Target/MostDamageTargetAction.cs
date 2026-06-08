using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostDamageTarget", story: "Agent targets target with highest potential damage [SelectedTarget]", category: "Target", id: "11319f87a6c8fa60658bc328ba54d93a")]
public partial class MostDamageTargetAction : Action
{
    private TargetComponent _targetComponent;
    private GameManager _gameManager;
    private SkillComponent _skillComponent;

    [SerializeReference] public BlackboardVariable<GameObject> SelectedTarget;

    protected override Status OnStart()
    {
        _targetComponent = GameObject.GetComponent<TargetComponent>();
        _skillComponent = GameObject.GetComponent<SkillComponent>();
        _gameManager = UnityEngine.Object.FindAnyObjectByType<GameManager>();

        if (_targetComponent == null || _gameManager == null || _skillComponent == null || _skillComponent.CurrentSkill == null)
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

        bool hasAttackDice = false;
        foreach (var dice in _skillComponent.CurrentSkill.Dice)
        {
            if (dice.Type == DiceType.Attack)
            {
                hasAttackDice = true;
                break;
            }
        }

        if (!hasAttackDice)
            return Status.Failure;

        GameObject selectedCharacter = null;
        float bestScore = float.MinValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            StatsComponent statsComponent = target.GetComponent<StatsComponent>();
            if (statsComponent == null)
                continue;

            float score = GetPotentialDamage(statsComponent, true);
            if (selectedCharacter == null || score > bestScore)
            {
                bestScore = score;
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

    private float GetPotentialDamage(StatsComponent targetStats, bool useMaxValues)
    {
        float total = 0f;

        foreach (var dice in _skillComponent.CurrentSkill.Dice)
        {
            if (dice.Type != DiceType.Attack)
                continue;

            int baseValue = useMaxValues ? dice.MaxValue : dice.MinValue;
            total += baseValue * targetStats.GetDamageMultiplier(dice.DamageType, _skillComponent.CurrentSkill.DamageAffinity);
        }

        return total;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

