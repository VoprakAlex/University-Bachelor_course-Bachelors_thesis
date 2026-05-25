using System.Collections.Generic;
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
}