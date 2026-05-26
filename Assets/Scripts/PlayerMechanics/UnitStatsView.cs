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

    [Header("Components")]
    [SerializeField] public StatsComponent _statsComponent;
    [SerializeField] public HealthComponent _healthComponent;
    [SerializeField] public StaggerComponent _staggerComponent;
    [SerializeField] public SanityComponent _sanityComponent;
    [SerializeField] public SpeedComponent _speedComponent;

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