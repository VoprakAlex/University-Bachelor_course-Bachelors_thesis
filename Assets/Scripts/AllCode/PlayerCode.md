using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public GameObject PlayerObject;

    [Header("Component")]
    [SerializeField] StatsComponent _statsComponent;
    [SerializeField] HandComponent _handComponent;
    [SerializeField] TargetComponent _targetComponent;
    [SerializeField] SkillComponent _skillComponent;

    [Header("Skill")]
    [SerializeField] public SkillView SkillPrefab;

    [Header("Hand")]
    [SerializeField] public Transform HandContainer;
    [SerializeField] public DynamicSpacing Spacing;

    [Header("Cards")]
    [SerializeField] public List<SkillView> RenderedCards = new();

    [Header("ControlEvents")]
    public UnityEvent DrawedAllCards;
    public UnityEvent AddedCard;
    public UnityEvent RemovedCard;
    public UnityEvent ClearedAllCards;
    public UnityEvent RefreshedCards;

    private void Start()
    {
        FillComponents();
        RefreshHandView();
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

        foreach (Skill skill in _handComponent.Hand)
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

    public void RefreshHandView()
    {
        StartCoroutine(RefreshRoutine());
    }

    private System.Collections.IEnumerator RefreshRoutine()
    {
        ClearAllCards();

        yield return null;

        DrawAllCards();

        RefreshedCards.Invoke();
    }
}

using UnityEngine;
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

        Debug.Log(CardCount);

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
}