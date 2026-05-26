using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UnitStatsView : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] public PlayerController _playerController;

    [Header("Text Fields")]
    [SerializeField] public TMP_Text NameText;
    [SerializeField] public TMP_Text CurrentHPText;
    [SerializeField] public TMP_Text MaxHPText;
    [SerializeField] public TMP_Text StaggerTresholdText;
    [SerializeField] public TMP_Text CurrentSPText;
    [SerializeField] public TMP_Text MaxSPText;
    [SerializeField] public TMP_Text MinSpeedText;
    [SerializeField] public TMP_Text MaxSpeedText;
    [SerializeField] public TMP_Text CurrentSpeedText;

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
    }

    public void Show()
    {
        RefreshTarget();

        UpdateAll();

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

        UpdateCurrentHP();
        UpdateMaxHP();

        UpdateStaggerThreshold();

        UpdateCurrentSP();
        UpdateMaxSP();

        UpdateMinSpeed();
        UpdateCurrentSpeed();
        UpdateMaxSpeed();


        UpdateDamageTypes();
        UpdateDamageAffinities();
    }

    public void UpdateName()
    {
        NameText.text = _statsComponent.Character.Name;
    }

    public void UpdateCurrentHP()
    {
        CurrentHPText.text = _healthComponent.CurrentHealth.ToString();
    }

    public void UpdateMaxHP()
    {
        MaxHPText.text = _statsComponent.MaxHP.ToString();
    }

    public void UpdateStaggerThreshold()
    {
        StaggerTresholdText.text = _staggerComponent.StaggerThreshold.ToString();
    }

    public void UpdateCurrentSP()
    {
        CurrentSPText.text = _sanityComponent.CurrentSanity.ToString();
    }

    public void UpdateMaxSP()
    {
        MaxSPText.text = _statsComponent.MaxSP.ToString();
    }

    public void UpdateMinSpeed()
    {
        MinSpeedText.text = _statsComponent.MinSpeed.ToString();
    }

    public void UpdateCurrentSpeed()
    {
        CurrentSpeedText.text = _speedComponent.CurrentSpeed.ToString();
    }

    public void UpdateMaxSpeed()
    {
        MaxSpeedText.text = _statsComponent.MaxSpeed.ToString();
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

        CurrentHPText.text = "";
        MaxHPText.text = "";

        StaggerTresholdText.text = "";

        CurrentSPText.text = "";
        MaxSPText.text = "";

        MinSpeedText.text = "";
        CurrentSpeedText.text = "";
        MaxSpeedText.text = "";

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
}