using System.Collections.Generic;
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

    [Header("Game Events")]
    public UnityEvent OnWin;
    public UnityEvent OnLose;

    private void Start()
    {
        RegisterAllObjects();
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


using System.Collections.Generic;
using UnityEngine;
using Unity.Behavior;
public class RoundManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] public  EventChannel alertChannel;

    public void StartRound()
    {
       

        alertChannel.SendEventMessage();
      
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class UnopposedHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject Attacker;
    [SerializeField] private GameObject Target;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        TargetComponent targetComponent = attacker.GetComponent<TargetComponent>();

        Target = targetComponent.MainTarget;
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity)
    {
        StatsComponent attackerStats = Attacker.GetComponent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponent<StatsComponent>();

        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage( rolledValue, dice.DamageType, affinity );

                break;

            //case DiceType.Defend:

                //attackerStats.IncreaseShield(rolledValue);

                //break;

            default:
                break;
        }
    }

    public void ExecuteUnopposedSkill(Skill skill)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            ExecuteUnopposedDice(currentDice, skill.DamageAffinity);

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteUnopposed(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent skillComponent = Attacker.GetComponent<SkillComponent>();

        ExecuteUnopposedSkill(skillComponent.CurrentSkill);
    }
}


using UnityEngine;

public class ClashHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public GameObject Attacker;
    [SerializeField] public GameObject Target;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        TargetComponent targetComponent = attacker.GetComponent<TargetComponent>();

        Target = targetComponent.MainTarget;
    }

    public bool ShouldRemove(Dice evade, Dice attack)
    {
        if (evade.Type == DiceType.Evade && attack.Type == DiceType.Attack)
        {
            return evade.RolledValue < attack.RolledValue;
        }

        return true;
    }

    public void ExecuteClashSkill(Skill attackerSkill, Skill targetSkill)
    {
        while (attackerSkill.Dice.Count > 0 && targetSkill.Dice.Count > 0)
        {
            Dice attackerDice = attackerSkill.Dice[0];
            Dice targetDice = targetSkill.Dice[0];

            ExecuteClashDice( attackerDice, targetDice, attackerSkill.DamageAffinity, targetSkill.DamageAffinity);

            if (ShouldRemove(attackerDice, targetDice))
            {
                attackerSkill.Dice.RemoveAt(0);
            }

            if (ShouldRemove(targetDice, attackerDice))
            {
                targetSkill.Dice.RemoveAt(0);
            }
        }

        StatsComponent attackerStats = Attacker.GetComponent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponent<StatsComponent>();

        if (attackerSkill.Dice.Count > 0)
        {
            ExecuteUnopposedSkill(attackerSkill, attackerStats, targetStats);
        }

        if (targetSkill.Dice.Count > 0)
        {
            ExecuteUnopposedSkill(targetSkill, targetStats, attackerStats);
        }
    }

    public void ExecuteClashDice( Dice attackerDice, Dice targetDice, DamageAffinity attackerAffinity, DamageAffinity targetAffinity)
    {
        StatsComponent attackerStats = Attacker.GetComponent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponent<StatsComponent>();

        int attackerRoll = attackerDice.Roll();
        int targetRoll = targetDice.Roll();

        // ATTACK vs ATTACK
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll > targetRoll)
                targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            else if (targetRoll > attackerRoll)
                attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);

            return;
        }

        // DEFEND vs DEFEND
        if (attackerDice.Type == DiceType.Defend && targetDice.Type == DiceType.Defend)
        {
            attackerStats.IncreaseShield(attackerRoll);
            targetStats.IncreaseShield(targetRoll);
            return;
        }

        // ATTACK vs DEFEND
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Defend)
        {
            if (targetRoll >= attackerRoll)
            {
                return;
            }

            targetStats.IncreaseShield(targetRoll);
            attackerStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Defend && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll >= targetRoll)
            {
                return;
            }

            attackerStats.IncreaseShield(attackerRoll);
            targetStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }

        
        // ATTACK vs EVADE
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Evade)
        {
            if (targetRoll >= attackerRoll)
                return;

            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Evade && targetDice.Type == DiceType.Attack)
        {
            if (attackerRoll >= targetRoll)
                return;

            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }

        // ATTACK vs COUNTER
        if (attackerDice.Type == DiceType.Attack && targetDice.Type == DiceType.Counter)
        {
            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            return;
        }

        if (attackerDice.Type == DiceType.Counter && targetDice.Type == DiceType.Attack)
        {
            targetStats.CalculateDamage(attackerRoll, attackerDice.DamageType, attackerAffinity);
            attackerStats.CalculateDamage(targetRoll, targetDice.DamageType, targetAffinity);
            return;
        }
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity, StatsComponent attackerStats, StatsComponent targetStats)
    {
        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage(rolledValue, dice.DamageType, affinity);

                break;

            //case DiceType.Defend:

            //attackerStats.IncreaseShield(rolledValue);

            //break;

            default:
                break;
        }
    }

    public void ExecuteUnopposedSkill(Skill skill, StatsComponent attackerStats, StatsComponent targetStats)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            ExecuteUnopposedDice(currentDice, skill.DamageAffinity, attackerStats, targetStats);

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteClash(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent attackerSkillComponent = Attacker.GetComponent<SkillComponent>();
        SkillComponent targetSkillComponent = Target.GetComponent<SkillComponent>();

        ExecuteClashSkill(attackerSkillComponent.CurrentSkill, targetSkillComponent.CurrentSkill);
    }
}

using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Handlers")]
    [SerializeField] public ClashHandler _clashHandler;
    [SerializeField] public UnopposedHandler _unopposedHandler;

    [Header("Attacker")]
    [SerializeField] public GameObject Attacker;

    private void Awake()
    {
        if (_clashHandler == null)
            _clashHandler = GetComponent<ClashHandler>();

        if (_unopposedHandler == null)
            _unopposedHandler = GetComponent<UnopposedHandler>();
    }

    public void SetAttacker(GameObject attacker)
    {
        Attacker = attacker;
    }

    public void ClearAttacker()
    {
        Attacker = null;
    }

    public void ExecuteCombat()
    {
        SkillComponent attackerSkillComponent = Attacker.GetComponent<SkillComponent>();
        Skill attackerSkill = attackerSkillComponent.CurrentSkill;

        if (attackerSkillComponent.IsClashing)
        {
            _clashHandler.ExecuteClash(Attacker);
        }
        else
        {
            _unopposedHandler.ExecuteUnopposed(Attacker);
        }
    }
}
