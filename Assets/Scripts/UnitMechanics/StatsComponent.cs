using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private SkillComponent _skillComponent;

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
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();
        if (_handComponent == null)
            _handComponent = GetComponent<HandComponent>();
        if (_skillComponent == null)
            _skillComponent = GetComponent<SkillComponent>();

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

        FindAnyObjectByType<RoundManager>().RoundEnd.AddListener(DrawSkill);

        Debug.Log("Awake");
    }

    private void Start()
    {
        _healthComponent.SetToMax();
        _sanityComponent.SetToMax();
        _shieldComponent.SetToStarting();
        _resistanceComponent.SetResistanceToStandard();
        _handComponent.RebuildDeck();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();

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

    public void DrawSkill()
    {
        _handComponent.DrawSkill();
    }

    public float GetDamageMultiplier(DamageType damageType, DamageAffinity affinity)
    {
        ResistanceTier typeTier = _resistanceComponent.CurrentDamageTypeResistances[damageType];
        ResistanceTier affinityTier = _resistanceComponent.CurrentDamageAffinityResistances[affinity];
        return typeTier.GetMultiplier() * affinityTier.GetMultiplier();
    }
}