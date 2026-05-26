using UnityEngine;
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
        //FillComponents();
        //RefreshHandView();
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
        _handComponent = PlayerObject.GetComponent<HandComponent>();
        _targetComponent = PlayerObject.GetComponent<TargetComponent>();
        _skillComponent = PlayerObject.GetComponent<SkillComponent>();
        _statsComponent = PlayerObject.GetComponent<StatsComponent>();
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
}