using UnityEngine;

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
    [SerializeField] private SkillRenderer _skillRenderer;

    private void Awake()
    {
        _skillRenderer = GetComponent<SkillRenderer>();
    }
    private void Start()
    {
        //FillComponents();
        //RefreshHandView();
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