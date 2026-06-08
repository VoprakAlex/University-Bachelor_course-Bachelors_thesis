using TMPro;
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
}