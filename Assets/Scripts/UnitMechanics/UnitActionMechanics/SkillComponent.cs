using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ActionComponent))]
public class SkillComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionComponent _actionComponent;

    [Header("Skill")]
    [field: SerializeField] public Skill CurrentSkill { get; private set; }
    [field: SerializeField] public bool IsClashing { get; private set; }
    [field: SerializeField] public List<Skill> UsedSkills { get; private set; } = new();

    [Header("Skill Events")]
    public UnityEvent<Skill> OnCurrentSkillSet;
    public UnityEvent OnCurrentSkillCleared;

    [Header("Clash Events")]
    public UnityEvent OnClashing;
    public UnityEvent OnNotClashing;

    [Header("Used Skills Events")]
    public UnityEvent<Skill> OnSkillAddedToUsed;
    public UnityEvent OnUsedSkillsCleared;

    private void Awake()
    {
        if (_actionComponent == null)
            _actionComponent = GetComponent<ActionComponent>();
    }

    public void SetCurrentSkill(Skill skill)
    {
        if (skill == null)
            return;

        CurrentSkill = skill;

        OnCurrentSkillSet?.Invoke(skill);
    }

    public void ClearCurrentSkill()
    {
        if (CurrentSkill == null)
            return;

        CurrentSkill = null;

        OnCurrentSkillCleared?.Invoke();
    }

    
    public void SetClashing()
    {
        if (IsClashing)
            return;

        IsClashing = true;

        OnClashing?.Invoke();
    }

    public void SetNotClashing()
    {
        if (!IsClashing)
            return;

        IsClashing = false;

        OnNotClashing?.Invoke();
    }

    public void AddCurrentSkillToUsed()
    {
        if (CurrentSkill == null)
            return;

        UsedSkills.Add(CurrentSkill);
        CurrentSkill = null;
        OnSkillAddedToUsed?.Invoke(CurrentSkill);
    }

    public void ClearUsedSkills()
    {
        if (UsedSkills.Count == 0)
            return;

        UsedSkills.Clear();

        OnUsedSkillsCleared?.Invoke();
    }
}