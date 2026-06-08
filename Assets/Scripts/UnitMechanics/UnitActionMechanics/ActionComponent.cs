using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(HandComponent), typeof(SkillComponent), typeof(TargetComponent))]
public class ActionComponent : MonoBehaviour
{
    [Header("Components")]
    private StatsComponent _statsComponent;
    private TargetComponent _targetComponent;
    private HandComponent _handComponent;
    private SkillComponent _skillComponent;

    public bool Ready { get; private set; }  = false;

    private void Awake()
    {
        if (_handComponent == null)
            _handComponent = GetComponent<HandComponent>();
        if (_skillComponent == null)
            _skillComponent = GetComponent<SkillComponent>();
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();

        if (_statsComponent == null)
            _statsComponent = GetComponentInParent<StatsComponent>();

        FindAnyObjectByType<RoundManager>().RoundEnd.AddListener(DrawSkill);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _handComponent.RebuildDeck();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();
        _handComponent.DrawSkill();

        Ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawSkill()
    {
        _handComponent.DrawSkill();
    }

    public List<SkillData> GetAllSkills()
    {
        return _statsComponent.AllSkills;
    }
}