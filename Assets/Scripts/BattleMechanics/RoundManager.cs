using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Unity.Behavior;

public class RoundManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] public GameManager GameManager;
    [SerializeField] public PlayerController PlayerController;
    [SerializeField] public CombatManager CombatManager;

    [Header("Events")]
    [SerializeField] public EventChannel TargetUpdateChannel;

    [Header("Round Queue")]
    [SerializeField] public List<GameObject> RoundQueue = new();

    [SerializeField] public List<GameObject> PlayedUnits = new();

    [Header("Current")]
    [SerializeField] public GameObject CurrentUnit;

    [SerializeField] public bool IsRoundPrepared;

    [Header("TurnEvents")]
    public UnityEvent RoundEnd;

    public void Start()
    {
        
    }

    private void Awake()
    {
        if (CombatManager != null)
        {
            CombatManager.CombatEnd.AddListener(EndTurn);
        }
    }

    private void OnDestroy()
    {
        if (CombatManager != null)
        {
            CombatManager.CombatEnd.RemoveListener(EndTurn);
        }
    }

    public void PrepareRound()
    {
        BuildRoundQueue();

        IsRoundPrepared = true;
        TargetUpdateChannel.SendEventMessage();
    }

    public void EndRound()
    {
        Debug.Log("EndRound");

        CurrentUnit = null;

        RoundQueue.Clear();

        PlayedUnits.Clear();

        RoundEnd.Invoke();
    }

    public void BuildRoundQueue()
    {
        RoundQueue.Clear();

        List<GameObject> AliveUnits = new(GameManager.AllAlive);

        foreach (GameObject Unit in AliveUnits)
        {
            StaggerComponent Stagger = Unit.GetComponent<StaggerComponent>();

            if (Stagger != null && Stagger.IsStaggered)
                continue;

            StatsComponent Stats = Unit.GetComponent<StatsComponent>();

            Stats.RandomSpeed();

            RoundQueue.Add(Unit);
        }

        RoundQueue = RoundQueue
            .OrderByDescending(Unit =>
                Unit.GetComponent<SpeedComponent>().CurrentSpeed)
            .ThenBy(Unit => GetTagPriority(Unit.tag))
            .ToList();
    }

    public void NextUnit()
    {
        Debug.Log("NextUnit");

        if (RoundQueue.Count == 0)
        {
            EndRound();

            return;
        }

        CurrentUnit = RoundQueue[0];

        RoundQueue.RemoveAt(0);

        PlayedUnits.Add(CurrentUnit);

        if (CurrentUnit.CompareTag(GameManager.PlayerTag))
        {
            StartPlayerTurn(CurrentUnit);
        }
        else
        {
            StartAITurn(CurrentUnit);
        }
    }

    public void StartBattle()
    {
        if (CurrentUnit == null)
            return;

        if (CurrentUnit.CompareTag(GameManager.PlayerTag) && PlayerController != null)
        {
            PlayerController.ClearAllCards();
            PlayerController.InvokeClearStats();
            PlayerController.DisableTargetChoosing();
        }

        CombatManager.SetAttacker(CurrentUnit);
        CombatManager.ExecuteCombat();
    }

    public void StartRound()
    {
        if (!IsRoundPrepared)
            return;

        IsRoundPrepared = false;
        NextUnit();
    }

    public void StartPlayerTurn(GameObject player)
    {
        if (PlayerController == null)
            return;

        PlayerController.PlayerObject = player;
        PlayerController.FillComponents();
        PlayerController.DisableTargetChoosing();

        PlayerController.RefreshHandView();
        PlayerController.InvokeShowStats();
    }

    public void EndTurn()
    {
        Debug.Log("EndTurn");

        SkillComponent skillComponent = CurrentUnit.GetComponent<SkillComponent>();
        if (skillComponent != null)
        {
            skillComponent.ClearCurrentSkill();
            skillComponent.SetNotClashing();
        }

        TargetComponent targetComponent = CurrentUnit.GetComponent<TargetComponent>();
        if (targetComponent != null)
        {
            targetComponent.ClearTargets();
        }

        CurrentUnit = null;

        NextUnit();
    }

    public void StartAITurn(GameObject Unit)
    {
        CombatManager.SetAttacker(Unit);
        CombatManager.ExecuteCombat();
    }

    public int GetTagPriority(string Tag)
    {
        switch (Tag)
        {
            case GameManager.AllyTag:
                return 0;

            case GameManager.AllyImportantTag:
                return 1;

            case GameManager.PlayerTag:
                return 2;

            case GameManager.EnemyTag:
                return 3;

            case GameManager.EnemyBossTag:
                return 4;

            default:
                return 999;
        }
    }
}
