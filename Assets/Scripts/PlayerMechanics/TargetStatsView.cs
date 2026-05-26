using TMPro;
using UnityEngine;

public class TargetStatsView : MonoBehaviour
{
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

    private StatsComponent _statsComponent;
    private HealthComponent _healthComponent;
    private StaggerComponent _staggerComponent;
    private SanityComponent _sanityComponent;
    private SpeedComponent _speedComponent;

    private GameObject CurrentTarget;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show(GameObject target)
    {
        if (target == null) return;

        CurrentTarget = target;

        RefreshTarget();
        UpdateAll();

        gameObject.SetActive(true);
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

        _statsComponent = CurrentTarget.GetComponent<StatsComponent>();
        _healthComponent = CurrentTarget.GetComponent<HealthComponent>();
        _staggerComponent = CurrentTarget.GetComponent<StaggerComponent>();
        _sanityComponent = CurrentTarget.GetComponent<SanityComponent>();
        _speedComponent = CurrentTarget.GetComponent<SpeedComponent>();
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
    }
}