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

    public void StartRound()
    {
        BuildRoundQueue();

        TargetUpdateChannel.SendEventMessage();
    }

    public IEnumerator WaitNextUnit()
    {
        yield return new WaitForSeconds(2f);

        NextUnit();
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


    public void StartPlayerTurn(GameObject Player)
    {
        CombatManager.SetAttacker(Player);
        CombatManager.ExecuteCombat();
        //PlayerController.PlayerObject = Player;

        //PlayerController.FillComponents();

        //PlayerController.RefreshHandView();
    }

    public void EndTurn()
    {
        Debug.Log("EndTurn");

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
