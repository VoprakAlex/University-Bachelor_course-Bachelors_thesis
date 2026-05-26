using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Tags")]
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    public const string EnemyBossTag = "EnemyBoss";
    public const string AllyTag = "Ally";
    public const string AllyImportantTag = "AllyImportant";

    [Header("Alive / Dead (Global)")]
    [SerializeField] public List<GameObject> AllAlive = new();
    [SerializeField] public List<GameObject> AllDead = new();

    [Header("Player")]
    [SerializeField] public List<GameObject> PlayerAlive = new();
    [SerializeField] public List<GameObject> PlayerDead = new();

    [Header("Enemy")]
    [SerializeField] public List<GameObject> EnemyAlive = new();
    [SerializeField] public List<GameObject> EnemyDead = new();

    [Header("Enemy Boss")]
    [SerializeField] public List<GameObject> EnemyBossAlive = new();
    [SerializeField] public List<GameObject> EnemyBossDead = new();

    [Header("Ally")]
    [SerializeField] public List<GameObject> AllyAlive = new();
    [SerializeField] public List<GameObject> AllyDead = new();

    [Header("Ally Important")]
    [SerializeField] public List<GameObject> AllyImportantAlive = new();
    [SerializeField] public List<GameObject> AllyImportantDead = new();

    [Header("State")]
    [SerializeField] public bool IsGameEnded;

    [SerializeField] public RoundManager RoundManager;

    [Header("Game Events")]
    public UnityEvent OnWin;
    public UnityEvent OnLose;

    private void Awake()
    {
        RegisterAllObjects();
    }

    private void Start()
    {
        StartCoroutine(WaitForAllUnitsReadyAndStartRound());
    }

    public IEnumerator WaitForAllUnitsReadyAndStartRound()
    {
        while (true)
        {
            bool allReady = true;

            foreach (var unit in AllAlive)
            {
                if (unit == null)
                    continue;

                StatsComponent stats = unit.GetComponent<StatsComponent>();

                if (stats == null || !stats.Ready)
                {
                    allReady = false;
                    break;
                }
            }

            if (allReady && AllAlive.Count > 0)
            {
                break;
            }

            yield return null;
        }

        RoundManager.PrepareRound();
    }

    public void RegisterAllObjects()
    {
        RegisterByTag(PlayerTag);
        RegisterByTag(EnemyTag);
        RegisterByTag(EnemyBossTag);
        RegisterByTag(AllyTag);
        RegisterByTag(AllyImportantTag);
    }

    public void RegisterByTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in objects)
        {
            RegisterObject(obj, tag);
        }
    }

    public void RegisterObject(GameObject obj, string tag)
    {
        HealthComponent health = obj.GetComponent<HealthComponent>();
        
        health.OnDeath.AddListener(() => OnObjectDeath(obj, tag));
        health.OnRevive.AddListener(() => OnObjectRevive(obj, tag));

        AddToAlive(obj, tag);
    }

    public void OnObjectDeath(GameObject obj, string tag)
    {
        if (IsGameEnded) return;

        MoveToDead(obj, tag);
        EvaluateState();
    }

    public void OnObjectRevive(GameObject obj, string tag)
    {
        if (IsGameEnded) return;

        MoveToAlive(obj, tag);
        EvaluateState();
    }

    public void EvaluateState()
    {
        if (IsGameEnded) return;

        if (EnemyBossDead.Count > 0)
        {
            WinGame();
            return;
        }

        if (EnemyAlive.Count == 0)
        {
            WinGame();
            return;
        }

        if (AllyImportantDead.Count > 0)
        {
            LoseGame();
            return;
        }

        if (PlayerAlive.Count == 0)
        {
            LoseGame();
            return;
        }
    }

    public void WinGame()
    {
        if (IsGameEnded) return;

        IsGameEnded = true;
        Time.timeScale = 0f;

        Debug.Log("Win");

        OnWin?.Invoke();
    }

    public void LoseGame()
    {
        if (IsGameEnded) return;

        IsGameEnded = true;
        Time.timeScale = 0f;

        Debug.Log("Lose");

        OnLose?.Invoke();
    }

    public void AddToAlive(GameObject obj, string tag)
    {
        AllAlive.Add(obj);

        switch (tag)
        {
            case PlayerTag:
                PlayerAlive.Add(obj);
                break;

            case EnemyTag:
                EnemyAlive.Add(obj);
                break;

            case EnemyBossTag:
                EnemyBossAlive.Add(obj);
                break;

            case AllyTag:
                AllyAlive.Add(obj);
                break;

            case AllyImportantTag:
                AllyImportantAlive.Add(obj);
                break;
        }
    }

    public void MoveToDead(GameObject obj, string tag)
    {
        RemoveFromAlive(obj, tag);

        if (!AllDead.Contains(obj))
            AllDead.Add(obj);

        switch (tag)
        {
            case PlayerTag:
                PlayerDead.Add(obj);
                break;

            case EnemyTag:
                EnemyDead.Add(obj);
                break;

            case EnemyBossTag:
                EnemyBossDead.Add(obj);
                break;

            case AllyTag:
                AllyDead.Add(obj);
                break;

            case AllyImportantTag:
                AllyImportantDead.Add(obj);
                break;
        }
    }

    public void MoveToAlive(GameObject obj, string tag)
    {
        RemoveFromDead(obj, tag);

        if (!AllAlive.Contains(obj))
            AllAlive.Add(obj);

        switch (tag)
        {
            case PlayerTag:
                PlayerAlive.Add(obj);
                break;

            case EnemyTag:
                EnemyAlive.Add(obj);
                break;

            case EnemyBossTag:
                EnemyBossAlive.Add(obj);
                break;

            case AllyTag:
                AllyAlive.Add(obj);
                break;

            case AllyImportantTag:
                AllyImportantAlive.Add(obj);
                break;
        }
    }

    public void RemoveFromAlive(GameObject obj, string tag)
    {
        AllAlive.Remove(obj);

        switch (tag)
        {
            case PlayerTag:
                PlayerAlive.Remove(obj);
                break;

            case EnemyTag:
                EnemyAlive.Remove(obj);
                break;

            case EnemyBossTag:
                EnemyBossAlive.Remove(obj);
                break;

            case AllyTag:
                AllyAlive.Remove(obj);
                break;

            case AllyImportantTag:
                AllyImportantAlive.Remove(obj);
                break;
        }
    }

    public void RemoveFromDead(GameObject obj, string tag)
    {
        AllDead.Remove(obj);

        switch (tag)
        {
            case PlayerTag:
                PlayerDead.Remove(obj);
                break;

            case EnemyTag:
                EnemyDead.Remove(obj);
                break;

            case EnemyBossTag:
                EnemyBossDead.Remove(obj);
                break;

            case AllyTag:
                AllyDead.Remove(obj);
                break;

            case AllyImportantTag:
                AllyImportantDead.Remove(obj);
                break;
        }
    }
}