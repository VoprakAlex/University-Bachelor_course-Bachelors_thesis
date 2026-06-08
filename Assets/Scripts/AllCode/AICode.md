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
using UnityEngine.Events;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(HealthComponent), typeof(StaggerComponent))]
public class StatsComponent : MonoBehaviour
{
    [Header("Components")]
    private HealthComponent _healthComponent;
    private ShieldComponent _shieldComponent;
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;
    private ResistanceComponent _resistanceComponent;
    private SpeedComponent _speedComponent;

    private CharacterStatsUI characterStatsUI;

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

    public bool Ready = false;

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
        if (characterStatsUI == null)
            characterStatsUI = GetComponent<CharacterStatsUI>();

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
    }

    private void Start()
    {
        _staggerComponent.SetStaggerAmount();
        _healthComponent.SetToMax();
        _sanityComponent.SetToMax();
        _shieldComponent.SetToStarting();
        _resistanceComponent.SetResistanceToStandard();

        characterStatsUI.InitializeUI();

        Ready = true;
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

    public void IncreaseShield(int amount)
    {
        _shieldComponent.IncreaseShield(amount);
    }

    public float GetDamageMultiplier(DamageType damageType, DamageAffinity affinity)
    {
        ResistanceTier typeTier = _resistanceComponent.CurrentDamageTypeResistances[damageType];
        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];
        return typeTier.GetMultiplier() * affinityTier.GetMultiplier();
    }
}using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimatorController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private HealthComponent healthComponent;

    [Header("Animator Parameters")]
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string hitTrigger = "Hit";
    [SerializeField] private string deathTrigger = "Death";

    [Header("Death")]
    [SerializeField] private float AnimationDuration = 1.0f;

    private bool isDead;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (healthComponent == null)
            healthComponent = GetComponentInParent<HealthComponent>();
    }

    private void OnEnable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath.AddListener(PlayDeath);
        }
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnDeath.RemoveListener(PlayDeath);
        }
    }

    public void PlayAttack()
    {
        if (isDead) return;

        animator.SetTrigger(attackTrigger);
        StartCoroutine(AttackAnimation());
    }

    public void PlayHit()
    {
        if (isDead) return;

        animator.SetTrigger(hitTrigger);
        StartCoroutine(HitAnimation());
    }

    public void PlayDeath()
    {
        if (isDead) return;

        isDead = true;

        animator.SetTrigger(deathTrigger);

        StartCoroutine(DeathAnimation());
    }

    public IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

        gameObject.SetActive(false);
    }

    public IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

    }

    public IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(AnimationDuration);

    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private StatsComponent statsComponent;

    private HealthComponent healthComponent;
    private StaggerComponent staggerComponent;
    private SpeedComponent speedComponent;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider staggerSlider;

    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text speedText;

    [SerializeField] private TMP_Text nameText;

    private void Awake()
    {
        if (statsComponent == null)
            statsComponent = GetComponent<StatsComponent>();

        healthComponent = statsComponent.GetComponent<HealthComponent>();
        staggerComponent = statsComponent.GetComponent<StaggerComponent>();
        speedComponent = statsComponent.GetComponent<SpeedComponent>();
    }

    private void OnEnable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnSetHealth.AddListener(UpdateHP);
            healthComponent.OnSetMaxHealth.AddListener(SetMaxHP);
        }

        if (staggerComponent != null)
        {
            staggerComponent.OnSetStaggerThreshold.AddListener(UpdateStagger);
            staggerComponent.OnDecreaseStaggerThreshold.AddListener(UpdateStagger);
            staggerComponent.OnIncreaseStaggerThreshold.AddListener(UpdateStagger);
        }

        if (speedComponent != null)
        {
            speedComponent.OnSetSpeed.AddListener(UpdateSpeed);
            speedComponent.OnIncreaseSpeed.AddListener(UpdateSpeed);
            speedComponent.OnDecreaseSpeed.AddListener(UpdateSpeed);
        }
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.OnSetHealth.RemoveListener(UpdateHP);
            healthComponent.OnSetMaxHealth.RemoveListener(SetMaxHP);
        }

        if (staggerComponent != null)
        {
            staggerComponent.OnSetStaggerThreshold.RemoveListener(UpdateStagger);
            staggerComponent.OnDecreaseStaggerThreshold.RemoveListener(UpdateStagger);
            staggerComponent.OnIncreaseStaggerThreshold.RemoveListener(UpdateStagger);
        }

        if (speedComponent != null)
        {
            speedComponent.OnSetSpeed.RemoveListener(UpdateSpeed);
            speedComponent.OnIncreaseSpeed.RemoveListener(UpdateSpeed);
            speedComponent.OnDecreaseSpeed.RemoveListener(UpdateSpeed);
        }
    }

    public void InitializeUI()
    {
        nameText.text = statsComponent.Character.name;

        SetMaxHP(statsComponent.MaxHP);

        UpdateHP(healthComponent.CurrentHealth);
        UpdateSpeed(speedComponent.CurrentSpeed);
        UpdateStagger(staggerComponent.StaggerThreshold);
    }

    private void SetMaxHP(int maxHP)
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = healthComponent.CurrentHealth;

        hpText.text = $"{healthComponent.CurrentHealth}";
    }

    private void UpdateHP(int currentHP)
    {
        hpSlider.value = currentHP;
        hpText.text = $"{currentHP}";
    }

    private void UpdateSpeed(int speed)
    {
        speedText.text = speed.ToString();
    }

    private void UpdateStagger(int threshold)
    {
        staggerSlider.maxValue = statsComponent.MaxHP;
        staggerSlider.value = threshold;
    }
}
using UnityEngine;
using TMPro;

public class ValueChange : MonoBehaviour
{
    [SerializeField] public TMP_Text Value;

    public void ClearText()
    {
        Value.text = "";
    }

    public void SetText(string text)
    {
        Value.text = text;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearText();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}using UnityEngine;
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
}using UnityEngine;
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
}using UnityEngine;
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
}using UnityEngine;
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
}using UnityEngine;
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

    public void SetStaggerAmount()
    {
        StaggerAmount = Mathf.RoundToInt(_statsComponent.MaxHP * _statsComponent.StaggerPercent);
        Debug.Log(StaggerAmount);
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
}using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(HandComponent), typeof(SkillComponent), typeof(TargetComponent))]
public class ActionComponent : MonoBehaviour
{
    [Header("Components")]
    private StatsComponent _statsComponent;
    private TargetComponent _targetComponent;
    private HandComponent _handComponent;
    private SkillComponent _skillComponent;

    public bool Ready { get; private set; }  = false;

    private void Awake()
    {
        if (_handComponent == null)
            _handComponent = GetComponent<HandComponent>();
        if (_skillComponent == null)
            _skillComponent = GetComponent<SkillComponent>();
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();

        if (_statsComponent == null)
            _statsComponent = GetComponentInParent<StatsComponent>();

        FindAnyObjectByType<RoundManager>().RoundEnd.AddListener(DrawSkill);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _handComponent.RebuildDeck();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();

        Ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawSkill()
    {
        _handComponent.DrawSkill();
    }

    public List<SkillData> GetAllSkills()
    {
        return _statsComponent.AllSkills;
    }
}using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(ActionComponent))]
public class HandComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionComponent _actionComponent;

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
        if (_actionComponent == null)
            _actionComponent = GetComponent<ActionComponent>();
    }

    public void RebuildDeck()
    {
        Deck.Clear();

        List<SkillData> source = _actionComponent.GetAllSkills();

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
}using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActionComponent))]
public class SkillComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionComponent _actionComponent;

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
        if (_actionComponent == null)
            _actionComponent = GetComponent<ActionComponent>();
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
}using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActionComponent))]
public class TargetComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionComponent _actionComponent;

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
        if (_actionComponent == null)
            _actionComponent = GetComponent<ActionComponent>();
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
}using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dice
{
    [Header("Data")]
    [field: SerializeField] public DiceData Data { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [Header("Values")]
    [field: SerializeField] public int MinValue { get; private set; }
    [field: SerializeField] public int MaxValue { get; private set; }
    [field: SerializeField] public int RolledValue { get; private set; }

    [Header("Types")]
    [field: SerializeField] public DiceType Type { get; private set; }
    [field: SerializeField] public DiceTargetType TargetType { get; private set; }
    [field: SerializeField] public DamageType DamageType { get; private set; }

    public Dice(DiceData source)
    {
        Data = source;
        Name = source.Name;
        Description = source.Description;
        MinValue = source.MinValue;
        MaxValue = source.MaxValue;
        Type = source.Type;
        TargetType = source.TargetType;
        DamageType = source.Damage;
    }

    public int Roll()
    {
        RolledValue = Random.Range(MinValue, MaxValue + 1);
        return RolledValue;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Dice", menuName = "Dice")]
public class DiceData : ScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;

    [Header("Values")]
    public int MinValue;
    public int MaxValue;

    [Header("Types")]
    public DiceType Type;
    public DiceTargetType TargetType;
    public DamageType Damage;
}
using System.Xml.Linq;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiceView : MonoBehaviour
{
    [Header("Data")]
    public Dice Data;

    [Header("Values")]
    public TMP_Text MinValue;
    public TMP_Text MaxValue;

    [Header("Image")]
    public Image DiceTypeImage;

    [Header("Sprites")]
    public Sprite Defend;
    public Sprite Evade;
    public Sprite Counter;

    public Sprite Slashing;
    public Sprite Piercing;
    public Sprite Bludgening;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDiceView();
    }

    public void SetDice(Dice dice)
    {
        Data = dice;
        UpdateDiceView();
    }

    public void UpdateDiceView()
    {
        MinValue.text = Data.MinValue.ToString();
        MaxValue.text = Data.MaxValue.ToString();

        switch (Data.Type)
        {
            case DiceType.Attack:
                switch (Data.DamageType)
                {
                    case DamageType.Slashing:
                        DiceTypeImage.sprite = Slashing;
                        break;

                    case DamageType.Piercing:
                        DiceTypeImage.sprite = Piercing;
                        break;

                    case DamageType.Bludgening:
                        DiceTypeImage.sprite = Bludgening;
                        break;
                }
                break;

            case DiceType.Defend:
                DiceTypeImage.sprite = Defend;
                break;

            case DiceType.Evade:
                DiceTypeImage.sprite = Evade;
                break;

            case DiceType.Counter:
                DiceTypeImage.sprite = Counter;
                break;
        } 
    }
}using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Skill
{
    [Header("Data")]
    [field: SerializeField] public SkillData Data { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [Header("Skill")]
    [field: SerializeField] public bool Clashable { get; private set; }
    [field: SerializeField] public DamageAffinity DamageAffinity { get; private set; }

    [Header("Dices")]
    [field: SerializeField] public List<Dice> Dice { get; private set; }

    public Skill(SkillData source)
    {
        Data = source;
        Name = source.Name;
        Description = source.Description;
        Clashable = source.Clashabale;
        DamageAffinity = source.DamageAffinity;
        Dice = source.Dice.Select(d => new Dice(d)).ToList();
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillController : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Refs")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SkillView _skillView;

    [Header("Hover")]
    [SerializeField] private float _hoverScale = 1.1f;

    private RectTransform _rectTransform;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = transform.localScale;

        _skillView = GetComponent<SkillView>();

        _playerController = FindAnyObjectByType<PlayerController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _defaultScale * _hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _defaultScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerController._skillComponent.SetCurrentSkill(_skillView.Data);

        _playerController._targetComponent?.ClearTargets(); 
        _playerController.EnableTargetChoosing();
        _playerController.InvokeShowStats();
    }
}using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillData : ScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;

    [Header("Skill")]
    public bool Clashabale = true;
    public DamageAffinity DamageAffinity;

    [Header("Dices")]
    public List<DiceData> Dice = new List<DiceData>();
}using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    [Header("Data")]
    public Skill Data;

    [Header("Images")]
    public Image CardBackground;
    public TMP_Text NameText;
    public TMP_Text StatusText;

    [Header("Dices")]
    public Transform DiceContainer;
    public DiceView DicePrefab;
    public List<DiceView> Dices = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSkillView();
    }

    public void SetSkill(Skill skill)
    {
        Data = skill;
        UpdateSkillView();
    }

    public void UpdateSkillView()
    {
        NameText.text = Data.Name;

        if (!Data.Clashable)
        {
            StatusText.gameObject.SetActive(true);
            StatusText.text = "Unclashable";
        }
        else
        {
            StatusText.gameObject.SetActive(false);
        }

        CardBackground.color = GetAffinityColor(Data.DamageAffinity);

        ClearDice();
        SpawnDice();
    }

    private void SpawnDice()
    {
        foreach (var dice in Data.Dice)
        {
            var view = Instantiate(DicePrefab, DiceContainer);
            view.SetDice(dice);
            Dices.Add(view);
        }
    }

    private void ClearDice()
    {
        foreach (var dice in Dices)
        {
            if (dice != null)
                Destroy(dice.gameObject);
        }

        Dices.Clear();
    }

    private Color GetAffinityColor(DamageAffinity affinity)
    {
        return affinity switch
        {
            DamageAffinity.Physical => new Color(0.69f, 0.69f, 0.69f),   // #B0B0B0
            DamageAffinity.Tremor => new Color(0.545f, 0.353f, 0.169f), // #8B5A2B
            DamageAffinity.Poison => new Color(0.298f, 0.737f, 0.318f),  // #4CAF50
            DamageAffinity.Bleed => new Color(0.717f, 0.109f, 0.109f),  // #B71C1C
            DamageAffinity.Electric => new Color(0f, 0.898f, 1f),        // #00E5FF
            DamageAffinity.Fire => new Color(1f, 0.239f, 0f),            // #FF3D00
            DamageAffinity.Cold => new Color(0.31f, 0.765f, 0.949f),     // #4FC3F7
            DamageAffinity.Mind => new Color(0.61f, 0.156f, 0.705f),     // #9C27B0
            _ => Color.white
        };
    }
}using UnityEngine;

[RequireComponent(typeof(ActionComponent))]
[RequireComponent(typeof(TargetComponent))]
public class ArcArrow2D : MonoBehaviour
{
    private TargetComponent _targetComponent;

    [Header("Visuals")]
    [SerializeField] private LineRenderer lr;

    [Header("Arc")]
    [Range(10, 50)]
    [SerializeField] private int segments = 20;

    [SerializeField] private float arcHeight = 2f;

    private void Awake()
    {
        _targetComponent = GetComponent<TargetComponent>();

        if (lr == null)
            lr = GetComponent<LineRenderer>();

        lr.enabled = false;
    }

    public void ShowArc()
    {
        if (_targetComponent == null || _targetComponent.MainTarget == null)
        {
            HideArc();
            return;
        }

        lr.enabled = true;
        Draw();
    }

    public void HideArc()
    {
        lr.enabled = false;
        lr.positionCount = 0;
    }

    private void Draw()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = _targetComponent.MainTarget.transform.position;

        DrawArc(startPos, endPos);
    }

    private void DrawArc(Vector3 p0, Vector3 p2)
    {
        Vector3 mid = (p0 + p2) * 0.5f;

        Vector3 dir = (p2 - p0).normalized;
        Vector3 perp = Vector3.Cross(dir, Vector3.forward);

        Vector3 up = Vector3.up;
        Vector3 p1 = mid + up * arcHeight;

        lr.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;

            Vector3 pos =
                Mathf.Pow(1 - t, 2) * p0 +
                2 * (1 - t) * t * p1 +
                Mathf.Pow(t, 2) * p2;

            lr.SetPosition(i, pos);
        }
    }
}using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicSpacing : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] public HorizontalLayoutGroup LayoutGroup;

    [Header("Container")]
    [SerializeField] public RectTransform Container;

    [Header("Settings")]
    [SerializeField] public int MaxCards = 24;

    private void Awake()
    {
        if (LayoutGroup == null)
            LayoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (Container == null)
            Container = GetComponent<RectTransform>();
    }

    public void RefreshSpacing()
    {
        int CardCount = transform.childCount;

        if (CardCount <= 6)
        {
            LayoutGroup.spacing = 0;
            return;
        }

        RectTransform FirstCard = transform.GetChild(0).GetComponent<RectTransform>();

        if (FirstCard == null)
            return;

        float AvailableWidth = Container.rect.width - FirstCard.rect.width * CardCount;

        LayoutGroup.spacing = AvailableWidth / (CardCount - 1);
    }
}using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public GameObject PlayerObject;

    [Header("Component")]
    [SerializeField] public StatsComponent _statsComponent;
    [SerializeField] public HandComponent _handComponent;
    [SerializeField] public TargetComponent _targetComponent;
    [SerializeField] public SkillComponent _skillComponent;

    [Header("Renderer")]
    [SerializeField] public SkillRenderer _skillRenderer;


    [SerializeField] public bool CanChooseTarget = false;

    [SerializeField] public RoundManager RoundManager;


    public UnityEvent ShowStats;
    public UnityEvent ClearStats;

    private void Awake()
    {
        _skillRenderer = GetComponent<SkillRenderer>();

        RoundManager = FindAnyObjectByType<RoundManager>();
    }

    private void Start()
    {
   
    }

    public void HandleActionButton()
    {
        if (RoundManager == null)
            return;

        if (RoundManager.IsRoundPrepared)
        {
            RoundManager.StartRound();
            return;
        }

        if (RoundManager.CurrentUnit != PlayerObject)
            return;

        if (_skillComponent == null || _skillComponent.CurrentSkill == null)
            return;

        if (_targetComponent == null || _targetComponent.MainTarget == null)
            return;

        _handComponent.Hand.Remove(_skillComponent.CurrentSkill);
        RoundManager.StartBattle();
    }

    public void InvokeShowStats()
    {
        ShowStats?.Invoke();
    }

    public void InvokeClearStats()
    {
        ClearStats?.Invoke();
    }

    public void EnableTargetChoosing()
    {
        CanChooseTarget = true;
    }

    public void DisableTargetChoosing()
    {
        CanChooseTarget = false;
    }

    public void FillComponents()
    {
        _statsComponent = PlayerObject.GetComponent<StatsComponent>();

        ActionComponent action =
            PlayerObject.GetComponentInChildren<ActionComponent>();

        _handComponent = action.GetComponent<HandComponent>();
        _targetComponent = action.GetComponent<TargetComponent>();
        _skillComponent = action.GetComponent<SkillComponent>();
    }

    public void DrawAllCards()
    {
        _skillRenderer.DrawAllCards(_handComponent.Hand);
    }

    public void AddCard(Skill skill)
    {
        _skillRenderer.AddCard(skill);
    }

    public void RemoveCard(SkillView card)
    {
        _skillRenderer.RemoveCard(card);
    }

    public void ClearAllCards()
    {
        _skillRenderer.ClearAllCards();
    }

    public void RefreshHandView()
    {
        _skillRenderer.RefreshHandView(_handComponent.Hand);
    }
}using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillRenderer : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] public SkillView SkillPrefab;

    [Header("Hand")]
    [SerializeField] public Transform HandContainer;
    [SerializeField] public DynamicSpacing Spacing;

    [Header("Cards")]
    [SerializeField] public List<SkillView> RenderedCards = new();

    [Header("RenderEvents")]
    public UnityEvent DrawedAllCards;
    public UnityEvent AddedCard;
    public UnityEvent RemovedCard;
    public UnityEvent ClearedAllCards;
    public UnityEvent RefreshedCards;

    public void DrawAllCards(List<Skill> skills)
    {
        foreach (Skill skill in skills)
        {
            AddCard(skill);
        }

        Spacing.RefreshSpacing();

        DrawedAllCards.Invoke();
    }

    public void AddCard(Skill skill)
    {
        if (skill == null)
            return;

        SkillView newCard = Instantiate(SkillPrefab, HandContainer);

        newCard.SetSkill(skill);

        RenderedCards.Add(newCard);

        AddedCard.Invoke();
    }

    public void RemoveCard(SkillView card)
    {
        RenderedCards.Remove(card);

        Destroy(card.gameObject);

        RemovedCard.Invoke();
    }

    public void ClearAllCards()
    {
        for (int i = RenderedCards.Count - 1; i >= 0; i--)
        {
            RemoveCard(RenderedCards[i]);
        }

        ClearedAllCards.Invoke();
    }

    public void RefreshHandView(List<Skill> skills)
    {
        StartCoroutine(RefreshRoutine(skills));
    }

    private System.Collections.IEnumerator RefreshRoutine(List<Skill> skills)
    {
        ClearAllCards();

        yield return null;

        DrawAllCards(skills);

        RefreshedCards.Invoke();
    }
}using TMPro;
using UnityEngine;

public class TargetStatsView : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] public TMP_Text NameText;
    

    [Header("Current Skill")]
    [SerializeField] private SkillView SkillView;

    [Header("Damage Type Multipliers")]
    [SerializeField] public TMP_Text SlashingMultiplierText;
    [SerializeField] public TMP_Text PiercingMultiplierText;
    [SerializeField] public TMP_Text BludgeoningMultiplierText;

    [Header("Damage Affinity Multipliers")]
    [SerializeField] public TMP_Text PhysicalMultiplierText;
    [SerializeField] public TMP_Text TremorMultiplierText;
    [SerializeField] public TMP_Text PoisonMultiplierText;
    [SerializeField] public TMP_Text BleedMultiplierText;
    [SerializeField] public TMP_Text ElectricMultiplierText;
    [SerializeField] public TMP_Text FireMultiplierText;
    [SerializeField] public TMP_Text ColdMultiplierText;
    [SerializeField] public TMP_Text MindMultiplierText;

    private StatsComponent _statsComponent;
    private HealthComponent _healthComponent;
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;
    private SpeedComponent _speedComponent;
    private ResistanceComponent _resistanceComponent;

    private ActionComponent _actionComponent;
    private SkillComponent _skillComponent;

    private GameObject CurrentTarget;

    [SerializeField] private GameObject Card;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(GameObject target)
    {
        if (target == null) return;

        CurrentTarget = target;

        gameObject.SetActive(true);

        RefreshTarget();
        UpdateAll();

       
    }

    public void Hide()
    {
        ClearTexts();
        gameObject.SetActive(false);
        CurrentTarget = null;
    }

    public void RefreshTarget()
    {
        if (CurrentTarget == null) return;

        _statsComponent = CurrentTarget.GetComponentInParent<StatsComponent>();

        _healthComponent = CurrentTarget.GetComponentInParent<HealthComponent>();

        _staggerComponent = CurrentTarget.GetComponentInParent<StaggerComponent>();

        _sanityComponent = CurrentTarget.GetComponentInParent<SanityComponent>();

        _speedComponent = CurrentTarget.GetComponentInParent<SpeedComponent>();

        _resistanceComponent = CurrentTarget.GetComponentInParent<ResistanceComponent>();

        _actionComponent = CurrentTarget.GetComponentInChildren<ActionComponent>();

        if (_actionComponent != null)
            _skillComponent = _actionComponent.GetComponent<SkillComponent>();
    }

    public void UpdateAll()
    {
        UpdateName();

        UpdateCurrentSkill();

        UpdateDamageTypes();
        UpdateDamageAffinities();
    }
    public void UpdateCurrentSkill()
    {
        if (SkillView == null)
            return;

        if (_skillComponent == null)
            return;

        if (_skillComponent.CurrentSkill == null || string.IsNullOrEmpty(_skillComponent.CurrentSkill.Name))
        {
            Card.SetActive(false);
            return;
        }
        else
        {
            SkillView.SetSkill(_skillComponent.CurrentSkill);
            Card.SetActive(true);
        }
    }
    public void UpdateName()
    {
        NameText.text = _statsComponent.Character.Name;
    }

    public void UpdateDamageTypes()
    {
        SlashingMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Slashing].GetMultiplier().ToString("0.00");

        PiercingMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Piercing].GetMultiplier().ToString("0.00");

        BludgeoningMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Bludgening].GetMultiplier().ToString("0.00");
    }

    public void UpdateDamageAffinities()
    {
        PhysicalMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Physical].GetMultiplier().ToString("0.00");

        TremorMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Tremor].GetMultiplier().ToString("0.00");

        PoisonMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Poison].GetMultiplier().ToString("0.00");

        BleedMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Bleed].GetMultiplier().ToString("0.00");

        ElectricMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Electric].GetMultiplier().ToString("0.00");

        FireMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Fire].GetMultiplier().ToString("0.00");

        ColdMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Cold].GetMultiplier().ToString("0.00");

        MindMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Mind].GetMultiplier().ToString("0.00");
    }

    public void ClearTexts()
    {
        NameText.text = "";

        

        SlashingMultiplierText.text = "";
        PiercingMultiplierText.text = "";
        BludgeoningMultiplierText.text = "";

        PhysicalMultiplierText.text = "";
        TremorMultiplierText.text = "";
        PoisonMultiplierText.text = "";
        BleedMultiplierText.text = "";
        ElectricMultiplierText.text = "";
        FireMultiplierText.text = "";
        ColdMultiplierText.text = "";
        MindMultiplierText.text = "";
    }
}using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UnitStatsView : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] public PlayerController _playerController;

    [Header("Text Fields")]
    [SerializeField] public TMP_Text NameText;

    [Header("Current Skill")]
    [SerializeField] private SkillView SkillView;

    [Header("Damage Type Multipliers")]
    [SerializeField] public TMP_Text SlashingMultiplierText;
    [SerializeField] public TMP_Text PiercingMultiplierText;
    [SerializeField] public TMP_Text BludgeoningMultiplierText;

    [Header("Damage Affinity Multipliers")]
    [SerializeField] public TMP_Text PhysicalMultiplierText;
    [SerializeField] public TMP_Text TremorMultiplierText;
    [SerializeField] public TMP_Text PoisonMultiplierText;
    [SerializeField] public TMP_Text BleedMultiplierText;
    [SerializeField] public TMP_Text ElectricMultiplierText;
    [SerializeField] public TMP_Text FireMultiplierText;
    [SerializeField] public TMP_Text ColdMultiplierText;
    [SerializeField] public TMP_Text MindMultiplierText;

    private StatsComponent _statsComponent;
    private HealthComponent _healthComponent;
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;
    private SpeedComponent _speedComponent;
    private ResistanceComponent _resistanceComponent;

    private ActionComponent _actionComponent;
    private SkillComponent _skillComponent;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();

        _playerController.ShowStats.AddListener(Show);
        _playerController.ClearStats.AddListener(Hide);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void RefreshTarget()
    {
        GameObject target = _playerController.PlayerObject;

        _statsComponent = target.GetComponent<StatsComponent>();
        _healthComponent = target.GetComponent<HealthComponent>();
        _staggerComponent = target.GetComponent<StaggerComponent>();
        _sanityComponent = target.GetComponent<SanityComponent>();
        _speedComponent = target.GetComponent<SpeedComponent>();
        _resistanceComponent = target.GetComponent<ResistanceComponent>();

        _actionComponent = target.GetComponentInChildren<ActionComponent>();

        if (_actionComponent != null)
            _skillComponent = _actionComponent.GetComponent<SkillComponent>();
    }

    public void Show()
    {
        RefreshTarget();

        UpdateAll();

        UpdateCurrentSkill();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        ClearTexts();

        gameObject.SetActive(false);
    }

    public void UpdateAll()
    {
        UpdateName();

        UpdateDamageTypes();
        UpdateDamageAffinities();
    }

    public void UpdateName()
    {
        NameText.text = _statsComponent.Character.Name;
    }

    public void UpdateDamageTypes()
    {
        SlashingMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Slashing].GetMultiplier().ToString("0.00");

        PiercingMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Piercing].GetMultiplier().ToString("0.00");

        BludgeoningMultiplierText.text =
            _resistanceComponent.CurrentDamageTypeResistances[DamageType.Bludgening].GetMultiplier().ToString("0.00");
    }

    public void UpdateDamageAffinities()
    {
        PhysicalMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Physical].GetMultiplier().ToString("0.00");

        TremorMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Tremor].GetMultiplier().ToString("0.00");

        PoisonMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Poison].GetMultiplier().ToString("0.00");

        BleedMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Bleed].GetMultiplier().ToString("0.00");

        ElectricMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Electric].GetMultiplier().ToString("0.00");

        FireMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Fire].GetMultiplier().ToString("0.00");

        ColdMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Cold].GetMultiplier().ToString("0.00");

        MindMultiplierText.text =
            _resistanceComponent.CurrentDamageAffinityResistances[DamageAffinity.Mind].GetMultiplier().ToString("0.00");
    }
    public void UpdateCurrentSkill()
    {
        if (SkillView == null)
            return;

        if (_skillComponent == null)
            return;

        if (_skillComponent.CurrentSkill == null)
        {
            SkillView.gameObject.SetActive(false);
            return;
        }

        SkillView.SetSkill(_skillComponent.CurrentSkill);
    }
    public void ClearTexts()
    {
        NameText.text = "";

        SlashingMultiplierText.text = "";
        PiercingMultiplierText.text = "";
        BludgeoningMultiplierText.text = "";

        PhysicalMultiplierText.text = "";
        TremorMultiplierText.text = "";
        PoisonMultiplierText.text = "";
        BleedMultiplierText.text = "";
        ElectricMultiplierText.text = "";
        FireMultiplierText.text = "";
        ColdMultiplierText.text = "";
        MindMultiplierText.text = "";
    }
}using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }
}using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueGameButton : MonoBehaviour
{
    private void Start()
    {
        if (!SaveSystem.HasSavedScene())
        {
            
            GetComponent<Button>().interactable = false;
        }
    }

    public void ContinueGame()
    {
        if (!SaveSystem.HasSavedScene())
            return;

        string lastScene = SaveSystem.GetLastScene();

        if (!string.IsNullOrEmpty(lastScene))
        {
            SceneManager.LoadScene(lastScene);
        }
    }
}using UnityEngine;
using TMPro;

public class FullscreenSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown fullscreenDropdown;

    private void Start()
    {
        fullscreenDropdown.value = PlayerPrefs.GetInt("FullscreenMode", 0);
        SetFullscreenMode(fullscreenDropdown.value);
    }

    public void SetFullscreenMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }

        PlayerPrefs.SetInt("FullscreenMode", mode);
        PlayerPrefs.Save();
    }
}using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject loseBackground;

    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnLose.AddListener(ShowLoseScreen);
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnLose.RemoveListener(ShowLoseScreen);
    }

    private void Start()
    {
        if (loseMenu != null) loseMenu.SetActive(false);
        if (loseBackground != null) loseBackground.SetActive(false);
    }

    public void ShowLoseScreen()
    {
        Debug.Log("Lose Screen - Show");

        loseMenu.SetActive(true);
        loseBackground.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideLoseScreen()
    {
        loseMenu.SetActive(false);
        loseBackground.SetActive(false);
    }
}
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;

        musicSource.loop = true;

        musicSource.Play();
    }
}using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject objectToHide;
    [SerializeField] private GameObject pauseBackground;

    private bool isPaused;

    private void Start()
    {
        if (pauseMenu != null && pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }

        if (pauseBackground != null && pauseBackground.activeSelf)
        {
            pauseBackground.SetActive(false);
        }
    }

    public void Pause()
    {

        Debug.Log("Pause - 1");
        pauseMenu.SetActive(true);
        pauseBackground.SetActive(true);
        Debug.Log("Pause - 2");

        objectToHide.SetActive(false);

        Debug.Log("Pause - 3");
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Debug.Log("Resume - 1");
        pauseMenu.SetActive(false);
        pauseBackground.SetActive(false);
        Debug.Log("Resume - 2");
        objectToHide.SetActive(true);

        Debug.Log("Resume - 3");
        Time.timeScale = 1f;
    }
}using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
}using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        resolutionDropdown.value = 0;
    }

    public void SetResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;

            case 1:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;

            case 2:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;

            case 3:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;

            case 4:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;

            case 5:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
        }
    }
}using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}using UnityEngine;

public static class SaveSystem
{
    private const string LastSceneKey = "LastScene";

    public static void SaveLastScene(string sceneName)
    {
        PlayerPrefs.SetString(LastSceneKey, sceneName);
        PlayerPrefs.Save();
        Debug.Log(LastSceneKey);
        Debug.Log(sceneName);
    }

    public static string GetLastScene()
    {
        Debug.Log(PlayerPrefs.GetString(LastSceneKey, ""));
        return PlayerPrefs.GetString(LastSceneKey, "");
    }

    public static bool HasSavedScene()
    {
        Debug.Log(PlayerPrefs.GetString(LastSceneKey, ""));
        return PlayerPrefs.HasKey(LastSceneKey);
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(LastSceneKey);
        PlayerPrefs.Save();
    }
}using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        
        if (currentScene == "MainMenu")
        {
            SaveSystem.DeleteSave();
        }


        if (sceneName != "MainMenu")
        {
            SaveSystem.SaveLastScene(sceneName);
        }

        SceneManager.LoadScene(sceneName);
    }
}using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject secondObjectToHide;

    private void Start()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings - 1");
        Debug.Log(settingsPanel);
        settingsPanel.SetActive(true);
        Debug.Log("OpenSettings - 2");
        secondObjectToHide.SetActive(false);
        Debug.Log("OpenSettings - 3");
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings - 1");
        Debug.Log(settingsPanel);
        settingsPanel.SetActive(false);
        Debug.Log("CloseSettings - 2");

        secondObjectToHide.SetActive(true);
        Debug.Log("CloseSettings - 3");
    }
}using UnityEngine;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject winBackground;

    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        if (gameManager != null)
            gameManager.OnWin.AddListener(ShowWinScreen);
    }

    private void OnDisable()
    {
        if (gameManager != null)
            gameManager.OnWin.RemoveListener(ShowWinScreen);
    }

    private void Start()
    {
        if (winMenu != null) winMenu.SetActive(false);
        if (winBackground != null) winBackground.SetActive(false);
    }

    public void ShowWinScreen()
    {
        Debug.Log("Win Screen - Show");

        winMenu.SetActive(true);
        winBackground.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideWinScreen()
    {
        winMenu.SetActive(false);
        winBackground.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class ActionStateView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TargetComponent _targetComponent;

    [Header("Renderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Sprite ReadySprite;

    private void Awake()
    {
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _targetComponent.OnMainTargetSet.AddListener(OnTargetChanged);
        _targetComponent.OnMainTargetCleared.AddListener(UpdateState);
    }

    private void OnDisable()
    {
        _targetComponent.OnMainTargetSet.RemoveListener(OnTargetChanged);
        _targetComponent.OnMainTargetCleared.RemoveListener(UpdateState);
    }

    private void OnTargetChanged(GameObject target)
    {
        UpdateState();
    }

    private void UpdateState()
    { 
        bool hasTarget = _targetComponent.MainTarget != null;

        if (!hasTarget)
        {
            _spriteRenderer.sprite = EmptySprite;
            return;
        }

        _spriteRenderer.sprite = ReadySprite;
    }
}using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float HoverScale = 1.1f;

    private Vector3 DefaultScale;

    private void Awake()
    {
        DefaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = DefaultScale * HoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = DefaultScale;
    }
}using UnityEngine;
using UnityEngine.EventSystems;

public class TargetActionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PlayerController _playerController;
    private TargetStatsView _targetStatsView;
    private ArcArrow2D _arcArrow;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _targetStatsView = FindAnyObjectByType<TargetStatsView>();
        _arcArrow = GetComponent<ArcArrow2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_playerController.CanChooseTarget)
            return;

        GameObject target = gameObject;

        // Назначаем цель игроку
        _playerController._targetComponent.SetMainTarget(target);

        // Компоненты игрока

        int playerSpeed =
            _playerController.PlayerObject.GetComponentInParent<SpeedComponent>().CurrentSpeed;

        // Компоненты цели
        int targetSpeed =
            target.GetComponentInParent<SpeedComponent>().CurrentSpeed;

        Debug.Log(targetSpeed);
        Debug.Log(playerSpeed);

        // Если цель медленнее игрока — начинается Clash
        if (targetSpeed < playerSpeed)
        {
            TargetComponent targetTarget =
                target.GetComponent<TargetComponent>();

            SkillComponent targetSkill =
                target.GetComponent<SkillComponent>();

            // Цель выбирает игрока своей целью
            targetTarget?.SetMainTarget(_playerController.PlayerObject.GetComponentInChildren<ActionComponent>().gameObject);

            // Clash у цели
            targetSkill?.SetClashing();

            // Clash у игрока
            _playerController._skillComponent.SetClashing();
        }

        UnitStatsView unitStatsView = FindAnyObjectByType<UnitStatsView>();
        if (unitStatsView != null)
        {
            unitStatsView.Hide();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetStatsView.Show(gameObject);
        _arcArrow?.ShowArc();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetStatsView.Hide();

        _arcArrow?.HideArc();
    }
}using UnityEngine;

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
        Debug.Log("-------");
        Debug.Log(attacker);
        Debug.Log(targetComponent.MainTarget);
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

            Debug.Log("-------");

            Debug.Log(attackerSkill.DamageAffinity);
            Debug.Log(targetSkill.DamageAffinity);

            ExecuteClashDice(attackerDice, targetDice, attackerSkill.DamageAffinity, targetSkill.DamageAffinity);

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

    public void ExecuteClashDice(Dice attackerDice, Dice targetDice, DamageAffinity attackerAffinity, DamageAffinity targetAffinity)
    {
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        int attackerRoll = attackerDice.Roll();
        int targetRoll = targetDice.Roll();
        Debug.Log("-------");
        Debug.Log(attackerDice.Type);
        Debug.Log(attackerDice.DamageType);

        Debug.Log(attackerAffinity);
        Debug.Log(attackerRoll);
        Debug.Log(targetDice.Type);
        Debug.Log(targetDice.DamageType);

        Debug.Log(targetAffinity);
        Debug.Log(targetRoll);



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

        Attacker = null;
        Target = null;

    }
}using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    [Header("Handlers")]
    [SerializeField] private GameObject UIToHideDuringCombat;
    [SerializeField] public ClashHandler _clashHandler;
    [SerializeField] public UnopposedHandler _unopposedHandler;

    [Header("Attacker")]
    [SerializeField] public GameObject Attacker;

    [Header("CombatEvents")]
    public UnityEvent CombatEnd;

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
        UIToHideDuringCombat.SetActive(false);
        ActionComponent[] actions = Attacker.GetComponentsInChildren<ActionComponent>();

        foreach (ActionComponent action in actions)
        {
            SkillComponent skillComponent = action.GetComponent<SkillComponent>();

            if (skillComponent == null)
                continue;

            if (skillComponent.CurrentSkill == null)
                continue;

            if (skillComponent.IsClashing)
            {
                _clashHandler.ExecuteClash(action.gameObject);

                RoundManager round = FindAnyObjectByType<RoundManager>();
                if (round != null)
                {
                    round.RoundQueue.Remove(action.GetComponent<TargetComponent>().MainTarget.transform.root.gameObject);
                }

                ClearCombatState(action.GetComponent<TargetComponent>().MainTarget);
                ClearCombatState(action.gameObject);

            }
            else
            {
                _unopposedHandler.ExecuteUnopposed(action.gameObject);
                ClearCombatState(action.gameObject);
            }
        }
        UIToHideDuringCombat.SetActive(true);
        CombatEnd.Invoke();
    }

    private void ClearCombatState(GameObject unit)
    {
        if (unit == null) return;

        ActionComponent[] actions = unit.GetComponentsInChildren<ActionComponent>();

        foreach (ActionComponent action in actions)
        {
            SkillComponent skill = action.GetComponent<SkillComponent>();
            if (skill != null)
            {
                skill.ClearCurrentSkill();
                skill.SetNotClashing();
            }

            TargetComponent target = action.GetComponent<TargetComponent>();
            if (target != null)
            {
                target.ClearTargets();
            }
        }
    }
}using System.Collections.Generic;
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
}ausing System.Collections.Generic;
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
        
        CurrentUnit = null;

        RoundQueue.Clear();

        PlayedUnits.Clear();

        RoundEnd.Invoke();
        IsRoundPrepared = false;
        PrepareRound();
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
    }

    public void EndTurn()
    {
        ActionComponent[] actions =
            CurrentUnit.GetComponentsInChildren<ActionComponent>();

        foreach (ActionComponent action in actions)
        {
            SkillComponent skill =
                action.GetComponent<SkillComponent>();

            if (skill != null)
            {
                skill.ClearCurrentSkill();
                skill.SetNotClashing();
            }

            TargetComponent target =
                action.GetComponent<TargetComponent>();

            if (target != null)
            {
                target.ClearTargets();
            }
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
using UnityEngine;
using System.Collections;

public class UnopposedHandler : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject Attacker;
    [SerializeField] private GameObject Target;

    private const float DiceDelay = 0.15f;
    private const float AnimationWait = 1.0f;

    public void SetObjects(GameObject attacker)
    {
        Attacker = attacker;

        Target = attacker.GetComponent<TargetComponent>().MainTarget;
    }

    public void ExecuteUnopposedDice(Dice dice, DamageAffinity affinity)
    {
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        int rolledValue = dice.Roll();

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage( rolledValue, dice.DamageType, affinity );

                break;
            default:
                break;
        }
    }

    public IEnumerator ExecuteUnopposedDiceCoroutine(Dice dice, DamageAffinity affinity)
    {
        Debug.Log("Couroutine Start");
        StatsComponent attackerStats = Attacker.GetComponentInParent<StatsComponent>();
        StatsComponent targetStats = Target.GetComponentInParent<StatsComponent>();

        CharacterAnimatorController attackerAnimator =  Attacker.GetComponentInParent<CharacterAnimatorController>();
        CharacterAnimatorController targetAnimator = Target.GetComponentInParent<CharacterAnimatorController>();

        ValueChange valueChange = Attacker.GetComponentInParent<ValueChange>();

        int rolledValue = dice.Roll();

        yield return new WaitForSeconds(0.1f);

        if (valueChange != null)
            valueChange.SetText(rolledValue.ToString());

        switch (dice.Type)
        {
            case DiceType.Attack:

                targetStats.CalculateDamage(rolledValue, dice.DamageType, affinity);

                break;
            default:
                break;
        }

        if (attackerAnimator != null)
            attackerAnimator.PlayAttack();

        if (targetAnimator != null)
            targetAnimator.PlayHit();

        yield return new WaitForSeconds(1.0f);

        if (valueChange != null)
            valueChange.ClearText();

        Debug.Log("Couroutine End");
    }

    public void ExecuteUnopposedSkill(Skill skill)
    {
        while (skill.Dice.Count > 0)
        {
            Dice currentDice = skill.Dice[0];

            StartCoroutine(ExecuteUnopposedDiceCoroutine(currentDice, skill.DamageAffinity));

            skill.Dice.RemoveAt(0);
        }
    }

    public void ExecuteUnopposed(GameObject attacker)
    {
        SetObjects(attacker);

        SkillComponent skillComponent = Attacker.GetComponent<SkillComponent>();

        ExecuteUnopposedSkill(skillComponent.CurrentSkill);

        Attacker = null;
        Target = null;
    }
}using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StartRound")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StartRound", message: "Target", category: "Events", id: "36052193b762e1ee0dd36ea8c70a11cc")]
public sealed partial class StartRound : EventChannel { }


using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayerMadnessCheck ", story: "If [GameObject] is Player, not staggered, and mad, continue.", category: "Flow", id: "023159a14d499ec71de9c230f9ffb874")]
public partial class PlayerMadnessCheckModifier : Modifier
{
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;

    protected override Status OnStart()
    {
        if (!CacheComponents())
            return Status.Failure;

        if (!CanContinue())
            return Status.Failure;

        if (Child == null)
            return Status.Success;

        Status childStatus = StartNode(Child);
        return childStatus == Status.Running || childStatus == Status.Waiting
            ? Status.Waiting
            : childStatus;
    }

    protected override Status OnUpdate()
    {
        return Child.CurrentStatus;
    }

    protected override void OnEnd()
    {
    }

    private bool CacheComponents()
    {
        if (GameObject == null)
            return false;

        if (_staggerComponent == null)
            _staggerComponent = GameObject.GetComponentInParent<StaggerComponent>();

        if (_sanityComponent == null)
            _sanityComponent = GameObject.GetComponentInParent<SanityComponent>();

        return _staggerComponent != null && _sanityComponent != null;
    }

    private bool CanContinue()
    {
        return GameObject.CompareTag("Player") && !_staggerComponent.IsStaggered && _sanityComponent.IsMad;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "AIMadnessCheck ", story: "If [GameObject] is mad", category: "Conditions", id: "47fb75d3b4ba036f11aae2782d72cf42")]
public partial class AiMadnessCheckCondition : Condition
{
    private SanityComponent _sanityComponent;

    public override bool IsTrue()
    {
        if (GameObject == null)
            return false;

        if (_sanityComponent == null)
            _sanityComponent = GameObject.GetComponentInParent<SanityComponent>();

        return _sanityComponent.IsMad;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
using System;
using Unity.Behavior;
using UnityEngine;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AICheck ", story: "If [GameObject] is not Player and not staggered, continue.", category: "Flow", id: "2f2dd20cec6da113a30d926f3232ab3d")]
public partial class AiCheckModifier : Modifier
{
    private StaggerComponent _staggerComponent;

    protected override Status OnStart()
    {
        if (!CacheComponents())
            return Status.Failure;

        if (!CanContinue())
            return Status.Failure;

        if (Child == null)
            return Status.Success;

        Status childStatus = StartNode(Child);
        return childStatus == Status.Running || childStatus == Status.Waiting
            ? Status.Waiting
            : childStatus;
    }

    protected override Status OnUpdate()
    {
        return Child.CurrentStatus;
    }

    protected override void OnEnd()
    {
    }

    private bool CacheComponents()
    {
        if (GameObject == null)
            return false;

        if (_staggerComponent == null)
            _staggerComponent = GameObject.GetComponentInParent<StaggerComponent>();

        return _staggerComponent != null;
    }

    private bool CanContinue()
    {
        return !GameObject.CompareTag("Player") && !_staggerComponent.IsStaggered;
    }
}

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FirstSkill", story: "Agent picks first skill in hand", category: "Skill", id: "7b9b53126adba3107cdb8dc4bf6b60dc")]
public partial class FirstSkillAction : Action
{

    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skill = action.GetComponent<SkillComponent>();

        if (hand == null || skill == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill firstSkill = hand.Hand[0];

        skill.SetCurrentSkill(firstSkill);
        hand.Hand.RemoveAt(0);

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

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastAttackDiceSkill", story: "Agent picks skill with least Attack dice", category: "Skill", id: "c5702200b60fa757ff8409174d782ef8")]
public partial class LeastAttackDiceSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill selectedSkill = hand.Hand
            .OrderBy(skill =>
                skill.Dice.Count(d => d.Type == DiceType.Attack))
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

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

using System;
using System.Linq;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastDefensiveDiceSkill", story: "Agent picks skill with least defensive dice", category: "Skill", id: "b11c7be355848edd4b1ee7f1dedff38a")]
public partial class LeastDefensiveDiceSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill selectedSkill = hand.Hand
            .OrderBy(GetDefensiveDiceCount)
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

        return Status.Success;
    }

    private int GetDefensiveDiceCount(Skill skill)
    {
        int count = 0;

        foreach (var dice in skill.Dice)
        {
            if (dice.Type == DiceType.Defend ||
                dice.Type == DiceType.Evade ||
                dice.Type == DiceType.Counter)
            {
                count++;
            }
        }

        return count;
    }

    protected override Status OnUpdate() => CurrentStatus;
    protected override void OnEnd() { }
}

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostAttackDiceSkill", story: "Agent picks skill with most Attack dice", category: "Skill", id: "b9cf27256e818909165facc0ccc6b35b")]
public partial class MostAttackDiceSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill selectedSkill = hand.Hand
            .OrderByDescending(skill =>
                skill.Dice.Count(d => d.Type == DiceType.Attack))
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

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

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Linq;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostDefensiveDiceSkill", story: "Agent picks skill with most defensive dice", category: "Skill", id: "0d0fa7cf46d3beed9b27c077401ee2a0")]
public partial class MostDefensiveDiceSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null || hand.Hand.Count == 0)
            return Status.Failure;

        Skill selectedSkill = hand.Hand
            .OrderByDescending(GetDefensiveDiceCount)
            .FirstOrDefault();

        if (selectedSkill == null)
            return Status.Failure;

        skillComponent.SetCurrentSkill(selectedSkill);
        hand.Hand.Remove(selectedSkill);

        return Status.Success;
    }

    private int GetDefensiveDiceCount(Skill skill)
    {
        int count = 0;

        foreach (var dice in skill.Dice)
        {
            if (dice.Type == DiceType.Defend ||
                dice.Type == DiceType.Evade ||
                dice.Type == DiceType.Counter)
            {
                count++;
            }
        }

        return count;
    }

    protected override Status OnUpdate() => CurrentStatus;
    protected override void OnEnd() { }
}

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PickSkill", story: "Agent picks [SkillData]", category: "Skill", id: "84412704f2848676b637383604e8dc93")]
public partial class PickSkillAction : Action
{
    [SerializeReference]
    public BlackboardVariable<SkillData> SkillData;

    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skillComponent = action.GetComponent<SkillComponent>();

        if (hand == null || skillComponent == null)
            return Status.Failure;

        if (SkillData?.Value == null)
            return Status.Failure;

        Skill skill = new Skill(SkillData.Value);

        skillComponent.SetCurrentSkill(skill);

        if (hand.Hand.Count > 0)
        {
            hand.Hand.RemoveAt(0);
        }

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

using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomSkill", story: "Agent picks random skill", category: "Skill", id: "31eb3b8db8a4def2861a69b7dcc92b64")]
public partial class RandomSkillAction : Action
{
    protected override Status OnStart()
    {
        ActionComponent action = GameObject.GetComponent<ActionComponent>();

        if (action == null)
            return Status.Failure;

        HandComponent hand = action.GetComponent<HandComponent>();
        SkillComponent skill = action.GetComponent<SkillComponent>();

        if (hand == null || skill == null || hand.Hand.Count == 0)
            return Status.Failure;

        int randomIndex = UnityEngine.Random.Range(0, hand.Hand.Count);
        Skill randomSkill = hand.Hand[randomIndex];

        skill.SetCurrentSkill(randomSkill);
        hand.Hand.RemoveAt(randomIndex);

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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FirstTarget", story: "Targets first enemy [SelectedTarget]", category: "Target", id: "f265da53b725f3348cd70c8264ffeff6")]
public partial class FirstTargetAction : Action
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
                return Status.Success;
        }

        if (possibleTargets.Count == 0)
            return Status.Failure;

        GameObject selectedCharacter = possibleTargets[0];

        ActionComponent[] actions =
            selectedCharacter.GetComponentsInChildren<ActionComponent>();

        if (actions.Length == 0)
            return Status.Failure;

        ActionComponent selectedAction = actions[0];

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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IndiscriminateTarget", story: "Targets random unit [SelectedTarget]", category: "Target", id: "79316144a327f8967d511cbe7545e330")]
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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastDamageTarget", story: "Agent targets target with lowest potential damage [SelectedTarget]", category: "Target", id: "6168fa28bd6f80bb79c0c29fb9285ccd")]
public partial class LeastDamageTargetAction : Action
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
        float bestScore = float.MaxValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            StatsComponent statsComponent = target.GetComponent<StatsComponent>();
            if (statsComponent == null)
                continue;

            float score = GetPotentialDamage(statsComponent, false);
            if (selectedCharacter == null || score < bestScore)
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

using System;

using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastSpeedTarget", story: "Agent targets slowest [SelectedTarget]", category: "Target", id: "8a6dc365b1b2dfab00712d7c32fe35ec")]
public partial class LeastSpeedTargetAction : Action
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
        int bestSpeed = int.MaxValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            SpeedComponent speedComponent = target.GetComponent<SpeedComponent>();
            if (speedComponent == null)
                continue;

            int currentSpeed = speedComponent.CurrentSpeed;
            if (selectedCharacter == null || currentSpeed < bestSpeed)
            {
                bestSpeed = currentSpeed;
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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeastStaggerThresholdTarget", story: "Agent targets lowest HP minus stagger threshold [SelectedTarget]", category: "Target", id: "2a51ab0c3722b5cb75ca2f234db39e19")]
public partial class LeastStaggerThresholdTargetAction : Action
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
        int bestDiff = int.MaxValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            StaggerComponent staggerComponent = target.GetComponent<StaggerComponent>();
            if (healthComponent == null || staggerComponent == null)
                continue;

            int diff = healthComponent.CurrentHealth - staggerComponent.StaggerThreshold;
            if (selectedCharacter == null || diff < bestDiff)
            {
                bestDiff = diff;
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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostHpTarget", story: "Agent targets highest HP [SelectedTarget]", category: "Target", id: "e20493659149d22d92535feeb1ed3d88")]
public partial class MostHpTargetAction : Action
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
        int bestHp = int.MinValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent == null)
                continue;

            int currentHp = healthComponent.CurrentHealth;
            if (selectedCharacter == null || currentHp > bestHp)
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

using System;

using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostSpeedTarget", story: "Agent targets fastest [SelectedTarget]", category: "Target", id: "955e1a448a8301920ae9b6bf0e3efd92")]
public partial class MostSpeedTargetAction : Action
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
        int bestSpeed = int.MinValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            SpeedComponent speedComponent = target.GetComponent<SpeedComponent>();
            if (speedComponent == null)
                continue;

            int currentSpeed = speedComponent.CurrentSpeed;
            if (selectedCharacter == null || currentSpeed > bestSpeed)
            {
                bestSpeed = currentSpeed;
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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MostStaggerThresholdTarget", story: "Agent targets highest HP minus stagger threshold [SelectedTarget]", category: "Target", id: "2304d801d6248546e3d353bdf00b1dcb")]
public partial class MostStaggerThresholdTargetAction : Action
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
        int bestDiff = int.MinValue;

        foreach (var target in possibleTargets)
        {
            if (target == null)
                continue;

            HealthComponent healthComponent = target.GetComponent<HealthComponent>();
            StaggerComponent staggerComponent = target.GetComponent<StaggerComponent>();
            if (healthComponent == null || staggerComponent == null)
                continue;

            int diff = healthComponent.CurrentHealth - staggerComponent.StaggerThreshold;
            if (selectedCharacter == null || diff > bestDiff)
            {
                bestDiff = diff;
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

using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RandomTarget", story: "Targets random enemy [SelectedTarget]", category: "Target", id: "601d1d600c3afd7b3217c617f71fe010")]
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

