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