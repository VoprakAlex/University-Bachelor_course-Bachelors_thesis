using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(StatsComponent))]
public class HandComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StatsComponent _statsComponent;

    [Header("Hand")]
    [SerializeField] public List<Skill> Hand = new();
    [SerializeField] public List<SkillData> Deck = new();

    [Header("Limits")]
    [SerializeField] private int MaxHandSize = 24;

    [Header("HandEvents")]
    public UnityEvent OnDeckRebuilt;
    public UnityEvent<Skill> OnSkillDrawn;

    private void Awake()
    {
        if (_statsComponent == null)
            _statsComponent = GetComponent<StatsComponent>();
    }

    public void RebuildDeck()
    {
        Deck.Clear();

        List<SkillData> source = _statsComponent.AllSkills;

        List<SkillData> shuffled = source
            .OrderBy(x => Random.value)
            .ToList();

        foreach (var skillData in shuffled)
        {
            Deck.Add(Instantiate(skillData));
        }

        OnDeckRebuilt?.Invoke();
    }

    public void DrawSkill()
    {
        if (Hand.Count >= MaxHandSize)
        {
            return;
        }

        if (Deck.Count == 0)
        {
            RebuildDeck();
        }

        SkillData drawn = Deck[0];
        Deck.RemoveAt(0);
        Skill skill = new Skill(drawn);
        Hand.Add(skill);

        OnSkillDrawn?.Invoke(skill);
    }
}