using UnityEngine;

public enum DamageType
{
    Slashing = 0,
    Piercing = 1,
    Bludgening = 2,
}

public enum DamageAffinity
{
    Physical = 0,   // #B0B0B0 (серый)
    Tremor = 1,     // #8B5A2B (землистый коричневый)
    Poison = 2,     // #4CAF50 (ядовито-зелёный)
    Bleed = 3,      // #B71C1C (тёмно-кроваво-красный)
    Electric = 4,   // #00E5FF (яркий неон-циан)
    Fire = 5,       // #FF3D00 (огненно-оранжево-красный)
    Cold = 6,       // #4FC3F7 (холодный голубой)
    Mind = 7,       // #9C27B0 (фиолетовый)
}

public enum ResistanceTier
{
    Immune = 0,       // multiplier 0.1f
    Ineffective = 1,  // multiplier 0.5f
    Resilient = 2,    // multiplier 0.75f
    Normal = 3,       // multiplier 1.0f
    Vulnerable = 4,   // multiplier 1.25f
    Effective = 5,    // multiplier 1.5f
    Fatal = 6         // multiplier 2.0f
}

public static class ResistanceData
{
    public static float GetMultiplier(this ResistanceTier tier)
    {
        return tier switch
        {
            ResistanceTier.Immune => 0.1f,
            ResistanceTier.Ineffective => 0.5f,
            ResistanceTier.Resilient => 0.75f,
            ResistanceTier.Normal => 1.0f,
            ResistanceTier.Vulnerable => 1.25f,
            ResistanceTier.Effective => 1.5f,
            ResistanceTier.Fatal => 2.0f,
            _ => 1.0f,
        };
    }
}

public enum DiceType
{
    Attack = 0,
    Defend = 1,
    Evade = 2,
    Counter = 3,
}

public enum DiceTargetType
{
    MainOnly = 0,
    AllTargets = 1,
    FirstSubTarget = 2,
    AllSubTargets = 3,
}

public enum SkillTargetType
{
    Enemies = 0,
    Allies = 1,
    Indiscriminate = 2,
    Self = 3,
}

using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterData : ScriptableObject
{
    [Header("Name")]
    public string Name;

    [Header("Characteristics")]
    public int MaxHP;
    public int MaxSP;
    public int StartingShield;
    public float StaggerPercent;

    [Header("Speed")]
    public int MinSpeed;
    public int MaxSpeed;

    [Header("Resistances")]
    [SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> StandardDamageTypeResistances = new SerializedDictionary<DamageType, ResistanceTier>
    {
        { DamageType.Slashing, ResistanceTier.Normal },
        { DamageType.Piercing, ResistanceTier.Normal },
        { DamageType.Bludgening, ResistanceTier.Normal }
    };

    [SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> StandardDamageAffinityResistances = new SerializedDictionary<DamageAffinity, ResistanceTier>
    {
        { DamageAffinity.Physical, ResistanceTier.Normal },
        { DamageAffinity.Tremor, ResistanceTier.Normal },
        { DamageAffinity.Poison, ResistanceTier.Normal },
        { DamageAffinity.Bleed, ResistanceTier.Normal },
        { DamageAffinity.Electric, ResistanceTier.Normal },
        { DamageAffinity.Fire, ResistanceTier.Normal },
        { DamageAffinity.Cold, ResistanceTier.Normal  } ,
        { DamageAffinity.Mind, ResistanceTier.Normal  }
    };

    [Header("Skills")]
    [SerializeField] public List<SkillData> Skills = new List<SkillData>();
}

using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(HealthComponent), typeof(StaggerComponent))]
public class StatsComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private ShieldComponent _shieldComponent;
    [SerializeField] private StaggerComponent _staggerComponent;
    [SerializeField] private SanityComponent _sanityComponent;
    [SerializeField] private ResistanceComponent _resistanceComponent;
    [SerializeField] private SpeedComponent _speedComponent;
    [SerializeField] private TargetComponent _targetComponent;
    [SerializeField] private HandComponent _handComponent;

    [Header("Character")]
    [SerializeField] public CharacterData Character;

    [Header("Characteristics")]
    public int MaxHP;
    public int MaxSP;
    public int StartingShield;
    public float StaggerPercent;

    [Header("Speed")]
    public int MinSpeed;
    public int MaxSpeed;

    [Header("Resistances")]
    [SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> StandardDamageTypeResistances;
    [SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> StandardDamageAffinityResistances;

    [Header("Skills")]
    [SerializeField] public List<SkillData> AllSkills = new List<SkillData>();

    [Header("StatsEvents")]
    public UnityEvent<int> TookHPDamage;
    public UnityEvent<int> TookSPDamage;
    public UnityEvent<int> Speed;
    public UnityEvent StartEnd;

    private void Awake()
    {
        if (_healthComponent == null)
            _healthComponent = GetComponent<HealthComponent>();
        if (_shieldComponent == null)
            _shieldComponent = GetComponent<ShieldComponent>();
        if (_staggerComponent == null)
            _staggerComponent = GetComponent<StaggerComponent>();
        if (_sanityComponent == null)
            _sanityComponent = GetComponent<SanityComponent>();
        if (_resistanceComponent == null)
            _resistanceComponent = GetComponent<ResistanceComponent>();
        if (_speedComponent == null)
            _speedComponent = GetComponent<SpeedComponent>();
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();
        if (_handComponent == null)
            _handComponent = GetComponent<HandComponent>();
    }

    void Start()
    {
        MaxHP = Character.MaxHP;
        MaxSP = Character.MaxSP;

        StartingShield = Character.StartingShield;

        StaggerPercent = Character.StaggerPercent;

        MinSpeed = Character.MinSpeed;
        MaxSpeed = Character.MaxSpeed;

        StandardDamageTypeResistances = new SerializedDictionary<DamageType, ResistanceTier>(Character.StandardDamageTypeResistances);
        StandardDamageAffinityResistances = new SerializedDictionary<DamageAffinity, ResistanceTier>(Character.StandardDamageAffinityResistances);

        foreach (var skillData in Character.Skills)
        {
            AllSkills.Add(Instantiate(skillData));
        }

        _healthComponent.SetToMax();
        _sanityComponent.SetToMax();
        _shieldComponent.SetToStarting();
        _resistanceComponent.SetResistanceToStandard();
        _handComponent.RebuildDeck();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();

        StartEnd.Invoke();
    }

    public int RandomSpeed()
    {
        _speedComponent.SetRandomSpeed();
        Speed.Invoke(_speedComponent.CurrentSpeed);
        return _speedComponent.CurrentSpeed;
    }

    public void TakeSPDamage(int amount)
    {
        _sanityComponent.DecreaseSanity(amount);
        TookSPDamage.Invoke(amount);
    }

    public void TakeHPDamage(int amount)
    {

        int remainingDamage = amount - _shieldComponent.CurrentShield;

        _shieldComponent.DecreaseShield(amount);

        if (remainingDamage > 0 && _shieldComponent.CurrentShield == 0)
        {
            _healthComponent.DecreaseHP(remainingDamage);
        }
        TookHPDamage.Invoke(amount);
    }

    public void CalculateSPDamage(int amount, DamageAffinity affinity)
    {
        if (affinity != DamageAffinity.Mind) return;

        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];

        float multiplier = affinityTier.GetMultiplier();

        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(amount * multiplier));

        TakeSPDamage(finalDamage);
    }

    public void CalculateHPDamage(int amount, DamageType type, DamageAffinity affinity)
    {
        float multiplier = GetDamageMultiplier(type, affinity);
        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(amount * multiplier));

        TakeHPDamage(finalDamage);
    }

    public void CalculateDamage(int amount, DamageType type, DamageAffinity affinity)
    {
        if (affinity == DamageAffinity.Mind)
        {
            CalculateSPDamage(amount, affinity);
        }
        else
        {
            CalculateHPDamage(amount, type, affinity);
        }
    }

    public float GetDamageMultiplier(DamageType damageType, DamageAffinity affinity)
    {
        ResistanceTier typeTier = _resistanceComponent.CurrentDamageTypeResistances[damageType];
        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];
        return typeTier.GetMultiplier() * affinityTier.GetMultiplier();
    }
}

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class HealthComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Health")]
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public bool IsDead { get; private set; }

    [Header("HealthEvents")]
    public UnityEvent<int> OnSetHealth;
    public UnityEvent<int> OnSetMaxHealth;
    public UnityEvent<int> OnDecreaseHP;
    public UnityEvent<int> OnIncreaseHP;

    [Header("DeathEvents")]
    public UnityEvent OnDeath;
    public UnityEvent OnRevive;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, _statsComponent.MaxHP);
        CurrentHealth = newHealth;
        OnSetHealth?.Invoke(newHealth);
    }

    public void SetToMax()
    {
        IsDead = false;
        SetHealth(_statsComponent.MaxHP);
        OnSetMaxHealth?.Invoke(_statsComponent.MaxHP);
    }

    public void DecreaseHP(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;

        SetHealth(CurrentHealth - amount);

        if (CurrentHealth == 0)
            Death();
        else
            OnDecreaseHP?.Invoke(CurrentHealth);
    }

    public void IncreaseHP(int amount)
    {
        if (IsDead) return;
        if (amount <= 0) return;
        if (CurrentHealth == _statsComponent.MaxHP) return;

        SetHealth(CurrentHealth + amount);
        OnIncreaseHP?.Invoke(CurrentHealth);
    }

    public void Revive(int amount)
    {
        if (!IsDead) return;
        if (amount <= 0) return;

        IsDead = false;
        SetHealth(amount);

        OnRevive?.Invoke();
    }

    public void Death()
    {
        if (IsDead) return;

        IsDead = true;

        OnDeath?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SanityComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Sanity")]
    [field: SerializeField] public int CurrentSanity { get; private set; }
    [field: SerializeField] public bool IsMad { get; private set; }

    [Header("SanityEvents")]
    public UnityEvent<int> OnSetSanity;
    public UnityEvent<int> OnDecreaseSanity;
    public UnityEvent<int> OnIncreaseSanity;
    
    [Header("MadnessEvents")]
    public UnityEvent OnMadness;
    public UnityEvent OnRecover;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetSanity(int newSanity)
    {
        newSanity = Mathf.Clamp(newSanity, 0, _statsComponent.MaxSP);
        CurrentSanity = newSanity;
        OnSetSanity?.Invoke(newSanity);
    }

    public void SetToMax()
    {
        IsMad = false;
        SetSanity(_statsComponent.MaxSP);
    }

    public void DecreaseSanity(int amount)
    {
        if (IsMad) return;
        if (amount <= 0) return;

        SetSanity(CurrentSanity - amount);

        if (CurrentSanity == 0)
            Madness();
        else
            OnDecreaseSanity?.Invoke(CurrentSanity);
    }

    public void IncreaseSanity(int amount)
    {
        if (IsMad) return;
        if (amount <= 0) return;
        if (CurrentSanity == _statsComponent.MaxSP) return;

        SetSanity(CurrentSanity + amount);
        OnIncreaseSanity?.Invoke(CurrentSanity);
    }

    public void Recover(int amount)
    {
        if (!IsMad) return;
        if (amount <= 0) return;

        IsMad = false;
        SetSanity(amount);

        OnRecover?.Invoke();
    }

    public void Madness()
    {
        if (IsMad) return;

        IsMad = true;

        OnMadness?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class ShieldComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Sheild")]
    [field: SerializeField] public int CurrentShield { get; private set; }

    [Header("SheildEvents")]
    public UnityEvent<int> OnSetShield;
    public UnityEvent<int> OnDecreaseShield;
    public UnityEvent<int> OnIncreaseShield;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }


    public void SetShield(int newShield)
    {
        newShield = Mathf.Max(newShield, 0);
        CurrentShield = newShield;
        OnSetShield?.Invoke(newShield);
    }

    public void SetToStarting()
    {
        SetShield(_statsComponent.StartingShield);
    }

    public void DecreaseShield(int amount)
    {
        if (amount <= 0) return;
        int newShield = CurrentShield - amount;
        SetShield(newShield);
        OnDecreaseShield?.Invoke(CurrentShield);
    }

    public void IncreaseShield(int amount)
    {
        if (amount <= 0) return;
        int newShield = CurrentShield + amount;
        SetShield(newShield);
        OnIncreaseShield?.Invoke(CurrentShield);
    }
}
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent), typeof(HealthComponent))]
public class StaggerComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;
    [SerializeField] private HealthComponent _healthComponent;

    [Header("Stagger")]
    [field: SerializeField] public bool IsStaggered { get; private set; }
    [field: SerializeField] public int StaggerAmount { get; private set; }
    [field: SerializeField] public int StaggerThreshold { get; private set; }

    [Header("StaggerThresholdEvents")]
    public UnityEvent<int> OnSetStaggerThreshold;
    public UnityEvent<int> OnDecreaseStaggerThreshold;
    public UnityEvent<int> OnIncreaseStaggerThreshold;

    [Header("StagerEvents")]
    public UnityEvent OnStagger;
    public UnityEvent OnUnstagger;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
        if (_healthComponent == null)
            _healthComponent = GetComponent<HealthComponent>();
    }

    private void OnEnable()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDecreaseHP.AddListener(CheckStagger);
            _healthComponent.OnIncreaseHP.AddListener(CheckStagger);
            _healthComponent.OnSetMaxHealth.AddListener(RefreshStaggerThreshold);
        }
        SetStaggerAmount();
    }

    private void OnDisable()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDecreaseHP.RemoveListener(CheckStagger);
            _healthComponent.OnIncreaseHP.RemoveListener(CheckStagger);
            _healthComponent.OnSetMaxHealth.RemoveListener(RefreshStaggerThreshold);
        }
    }

    private void RefreshStaggerThreshold(int currentHealth)
    {
        SetStaggerThreshold(currentHealth - StaggerAmount);
    }

    private void SetStaggerAmount()
    {
        StaggerAmount = Mathf.RoundToInt(_statsComponent.MaxHP * _statsComponent.StaggerPercent);
    }

    private void CheckStagger(int currentHealth)
    {
        if (!IsStaggered && currentHealth <= StaggerThreshold)
        {
            Stagger();
        }
    }

    public void SetStaggerThreshold(int newStaggerThreshold)
    {
        StaggerThreshold = Mathf.Max(0, newStaggerThreshold);
        OnSetStaggerThreshold?.Invoke(StaggerThreshold);
    }

    public void DecreaseStaggerThreshold(int amount)
    {
        if (IsStaggered) return;
        if (amount <= 0) return;

        SetStaggerThreshold(StaggerThreshold - amount);
        OnDecreaseStaggerThreshold?.Invoke(StaggerThreshold);
        CheckStagger(_healthComponent.CurrentHealth);
    }

    public void IncreaseStaggerThreshold(int amount)
    {
        if (IsStaggered) return;
        if (amount <= 0) return;

        SetStaggerThreshold(StaggerThreshold + amount);
        OnIncreaseStaggerThreshold?.Invoke(StaggerThreshold);
        CheckStagger(_healthComponent.CurrentHealth);
    }

    public void Stagger()
    {
        if (IsStaggered) return;
        IsStaggered = true;
        OnStagger?.Invoke();
    }

    public void Unstagger()
    {
        if (!IsStaggered) return;
        IsStaggered = false;
        
        RefreshStaggerThreshold(_healthComponent.CurrentHealth);

        OnUnstagger?.Invoke();
    }
}

using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent), typeof(StaggerComponent))]
public class ResistanceComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;
    [SerializeField] private StaggerComponent _staggerComponent;

    [Header("Resistances")]
    [field: SerializedDictionary("DamageType", "ResistanceTier")]
    public SerializedDictionary<DamageType, ResistanceTier> CurrentDamageTypeResistances //{ get; private set; }
        = new SerializedDictionary<DamageType, ResistanceTier>();

    [field: SerializedDictionary("DamageAffinity", "ResistanceTier")]
    public SerializedDictionary<DamageAffinity, ResistanceTier> CurrentDamageAffinityResistances //{ get; private set; }
        = new SerializedDictionary<DamageAffinity, ResistanceTier>();

    [Header("ResistancesEvents")]
    public UnityEvent OnSetDamageTypeResistancesToMax;
    public UnityEvent OnSetDamageTypeResistancesToStandard;
    public UnityEvent<DamageType, ResistanceTier> OnDamageTypeResistanceChanged;
    public UnityEvent<DamageAffinity, ResistanceTier> OnDamageAffinityResistanceChanged;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
        if (_staggerComponent == null)
            _staggerComponent = GetComponent<StaggerComponent>();
    }

    private void OnEnable()
    {
        _staggerComponent.OnStagger.AddListener(SetDamageTypeResistancesToMax);
        _staggerComponent.OnUnstagger.AddListener(SetDamageTypeResistancesToStandard);
    }

    private void OnDisable()
    {
        _staggerComponent.OnStagger.RemoveListener(SetDamageTypeResistancesToMax);
        _staggerComponent.OnUnstagger.RemoveListener(SetDamageTypeResistancesToStandard);
    }

    public void SetResistance(DamageType type, ResistanceTier tier)
    {
        CurrentDamageTypeResistances[type] = tier;
        OnDamageTypeResistanceChanged?.Invoke(type, tier);
    }

    public void SetResistance(DamageAffinity affinity, ResistanceTier tier)
    {
        CurrentDamageAffinityResistances[affinity] = tier;
        OnDamageAffinityResistanceChanged?.Invoke(affinity, tier);
    }

    public void SetAllResistanceTypes(ResistanceTier tier)
    {
        foreach (var type in System.Enum.GetValues(typeof(DamageType)))
        {
            DamageType dt = (DamageType)type;
            SetResistance(dt, tier);
        }
    }

    public void SetAllResistanceAffinity(ResistanceTier tier)
    {
        foreach (var affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            DamageAffinity da = (DamageAffinity)affinity;
            SetResistance(da, tier);
        }
    }

    public void SetDamageTypeResistancesToMax()
    {
        SetAllResistanceTypes(ResistanceTier.Fatal);
        OnSetDamageTypeResistancesToMax?.Invoke();
    }

    public void SetDamageAffinityResistancesToMax()
    {
        SetAllResistanceAffinity(ResistanceTier.Fatal);
    }

    public void SetDamageTypeResistancesToStandard()
    {
        foreach (var kvp in _statsComponent.StandardDamageTypeResistances)
            SetResistance(kvp.Key, kvp.Value);
        OnSetDamageTypeResistancesToStandard?.Invoke();
    }

    public void SetDamageAffinityResistancesToStandard()
    {
        foreach (var kvp in _statsComponent.StandardDamageAffinityResistances)
            SetResistance(kvp.Key, kvp.Value);
    }

    public void SetResistanceToStandard()
    {
        SetDamageTypeResistancesToStandard();
        SetDamageAffinityResistancesToStandard();
    }

    public void IncreaseResistance(DamageType type)
    {
        ResistanceTier currentTier = CurrentDamageTypeResistances[type];
        int nextTierValue = (int)currentTier + 1;

        if (nextTierValue < System.Enum.GetValues(typeof(ResistanceTier)).Length)
        {
            SetResistance(type, (ResistanceTier)nextTierValue);
        }
    }

    public void IncreaseResistance(DamageAffinity affinity)
    {
        ResistanceTier currentTier = CurrentDamageAffinityResistances[affinity];
        int nextTierValue = (int)currentTier + 1;

        if (nextTierValue < System.Enum.GetValues(typeof(ResistanceTier)).Length)
        {
            SetResistance(affinity, (ResistanceTier)nextTierValue);
        }
    }

    public void DecreaseResistance(DamageType type)
    {
        ResistanceTier currentTier = CurrentDamageTypeResistances[type];
        int nextTierValue = (int)currentTier - 1;

        if (nextTierValue >= 0)
        {
            SetResistance(type, (ResistanceTier)nextTierValue);
        }
    }

    public void DecreaseResistance(DamageAffinity affinity)
    {
        ResistanceTier currentTier = CurrentDamageAffinityResistances[affinity];
        int nextTierValue = (int)currentTier - 1;

        if (nextTierValue >= 0)
        {
            SetResistance(affinity, (ResistanceTier)nextTierValue);
        }
    }

    public void IncreaseAllDamageTypeResistances()
    {
        foreach (DamageType type in System.Enum.GetValues(typeof(DamageType)))
        {
            IncreaseResistance(type);
        }
    }

    public void IncreaseAllDamageAffinityResistances()
    {
        foreach (DamageAffinity affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            IncreaseResistance(affinity);
        }
    }

    public void DecreaseAllDamageTypeResistances()
    {
        foreach (DamageType type in System.Enum.GetValues(typeof(DamageType)))
        {
            DecreaseResistance(type);
        }
    }

    public void DecreaseAllDamageAffinityResistances()
    {
        foreach (DamageAffinity affinity in System.Enum.GetValues(typeof(DamageAffinity)))
        {
            DecreaseResistance(affinity);
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SpeedComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Speed")]
    [field: SerializeField] public int CurrentSpeed { get; private set; }

    [Header("SpeedEvents")]
    public UnityEvent<int> OnSetSpeed;
    public UnityEvent<int> OnIncreaseSpeed;
    public UnityEvent<int> OnDecreaseSpeed;

    private void Awake()
    {
        if (_statsComponent == null)
        {
            _statsComponent = GetComponent<StatsComponent>();
        }
    }

    public void SetSpeed(int newSpeed)
    {
        CurrentSpeed = Mathf.Max(newSpeed, 1);
        OnSetSpeed?.Invoke(newSpeed);
    }

    public void SetRandomSpeed()
    {
        int randomSpeed = Random.Range(_statsComponent.MinSpeed, _statsComponent.MaxSpeed);
        SetSpeed(randomSpeed);
    }

    public void IncreaseSpeed(int amount)
    {
        SetSpeed(CurrentSpeed + amount);
        OnIncreaseSpeed?.Invoke(CurrentSpeed);
    }

    public void DecreaseSpeed(int amount)
    {
        SetSpeed(CurrentSpeed - amount);
        OnDecreaseSpeed?.Invoke(CurrentSpeed);
    }
}
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class HandComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Hand")]
    [SerializeField] public List<Skill> Hand = new();
    [SerializeField] public List<SkillData> Deck = new();

    [Header("Limits")]
    [SerializeField] private int MaxHandSize = 24;

    [Header("HandEvents")]
    public UnityEvent OnDeckRebuilt;
    public UnityEvent<Skill> OnSkillDrawn;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void RebuildDeck()
    {
        Deck.Clear();

        List<SkillData> source = _statsComponent.AllSkills;

        List<SkillData> shuffled = source
            .OrderBy(x => Random.value)
            .ToList();

        foreach (var skillData in shuffled)
        {
            Deck.Add(Instantiate(skillData));
        }

        OnDeckRebuilt?.Invoke();
    }

    public void DrawSkill()
    {
        if (Hand.Count >= MaxHandSize)
        {
            return;
        }

        if (Deck.Count == 0)
        {
            RebuildDeck();
        }

        SkillData drawn = Deck[0];
        Deck.RemoveAt(0);
        Skill skill = new Skill(drawn);
        Hand.Add(skill);

        OnSkillDrawn?.Invoke(skill);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class TargetComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Target")]
    [field: SerializeField] public GameObject MainTarget { get; private set; }
    [SerializeField] public List<GameObject> SubTargets = new List<GameObject>();

    [Header("TargetEvents")]
    public UnityEvent<GameObject> OnMainTargetSet;
    public UnityEvent OnSubTargetsSet;
    public UnityEvent OnMainTargetCleared;
    public UnityEvent OnSubTargetCleared;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetMainTarget(GameObject target)
    {
        if (MainTarget == target) return;
        MainTarget = target;
        OnMainTargetSet?.Invoke(target);
    }

    public void FillSubTargets(int count, List<GameObject> possibleTargets)
    {
        if (count <= 0) return;
        if (possibleTargets == null)
        {
            SubTargets.Clear();
            return;
        }

        List<GameObject> availableTargets = new List<GameObject>(possibleTargets);

        if (MainTarget != null && availableTargets.Contains(MainTarget))
            availableTargets.Remove(MainTarget);

        int takeCount = Mathf.Min(count, availableTargets.Count);

        if (takeCount <= 0)
        {
            SubTargets.Clear();
            return;
        }

        List<GameObject> selected = availableTargets.GetRange(availableTargets.Count - takeCount, takeCount);

        SubTargets.Clear();
        SubTargets.AddRange(selected);
        OnSubTargetsSet?.Invoke();
    }

    public void ClearTargets()
    {
        ClearMainTarget();
        ClearSubTargets();
    }

    public void RemoveFirstSubTarget()
    {
        if (SubTargets.Count > 0)
        {
            SubTargets.RemoveAt(0);
        }
    }

    public void ClearMainTarget()
    {
        MainTarget = null;
        SubTargets.Clear();

        OnMainTargetCleared?.Invoke();
    }

    public void ClearSubTargets()
    {
        SubTargets.Clear();

        OnSubTargetCleared?.Invoke();
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class SkillComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Skill")]
    [field: SerializeField] public Skill CurrentSkill { get; private set; }
    [field: SerializeField] public bool IsClashing { get; private set; }
    [field: SerializeField] public List<Skill> UsedSkills { get; private set; } = new();

    [Header("Skill Events")]
    public UnityEvent<Skill> OnCurrentSkillSet;
    public UnityEvent OnCurrentSkillCleared;

    [Header("Clash Events")]
    public UnityEvent OnClashing;
    public UnityEvent OnNotClashing;

    [Header("Used Skills Events")]
    public UnityEvent<Skill> OnSkillAddedToUsed;
    public UnityEvent OnUsedSkillsCleared;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void SetCurrentSkill(Skill skill)
    {
        if (skill == null)
            return;

        CurrentSkill = skill;

        OnCurrentSkillSet?.Invoke(skill);
    }

    public void ClearCurrentSkill()
    {
        if (CurrentSkill == null)
            return;

        CurrentSkill = null;

        OnCurrentSkillCleared?.Invoke();
    }

    
    public void SetClashing()
    {
        if (IsClashing)
            return;

        IsClashing = true;

        OnClashing?.Invoke();
    }

    public void SetNotClashing()
    {
        if (!IsClashing)
            return;

        IsClashing = false;

        OnNotClashing?.Invoke();
    }

    public void AddCurrentSkillToUsed()
    {
        if (CurrentSkill == null)
            return;

        UsedSkills.Add(CurrentSkill);
        CurrentSkill = null;
        OnSkillAddedToUsed?.Invoke(CurrentSkill);
    }

    public void ClearUsedSkills()
    {
        if (UsedSkills.Count == 0)
            return;

        UsedSkills.Clear();

        OnUsedSkillsCleared?.Invoke();
    }
}