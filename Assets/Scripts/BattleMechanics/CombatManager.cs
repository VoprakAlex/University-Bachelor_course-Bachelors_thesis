using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    [Header("Handlers")]
    [SerializeField] private GameObject UIToHideDuringCombat;
    [SerializeField] public ClashHandler _clashHandler;
    [SerializeField] public UnopposedHandler _unopposedHandler;

    [Header("Attacker")]
    [SerializeField] public GameObject Attacker;

    [Header("CombatEvents")]
    public UnityEvent CombatEnd;

    private void Awake()
    {
        if (_clashHandler == null)
            _clashHandler = GetComponent<ClashHandler>();

        if (_unopposedHandler == null)
            _unopposedHandler = GetComponent<UnopposedHandler>();
    }

    public void SetAttacker(GameObject attacker)
    {
        Attacker = attacker;
    }

    public void ClearAttacker()
    {
        Attacker = null;
    }


    public void ExecuteCombat()
    {
        UIToHideDuringCombat.SetActive(false);
        ActionComponent[] actions = Attacker.GetComponentsInChildren<ActionComponent>();

        foreach (ActionComponent action in actions)
        {
            SkillComponent skillComponent = action.GetComponent<SkillComponent>();

            if (skillComponent == null)
                continue;

            if (skillComponent.CurrentSkill == null)
                continue;

            if (skillComponent.IsClashing)
            {
                _clashHandler.ExecuteClash(action.gameObject);

                RoundManager round = FindAnyObjectByType<RoundManager>();
                if (round != null)
                {
                    round.RoundQueue.Remove(action.GetComponent<TargetComponent>().MainTarget.transform.root.gameObject);
                }

                ClearCombatState(action.GetComponent<TargetComponent>().MainTarget);
                ClearCombatState(action.gameObject);

            }
            else
            {
                _unopposedHandler.ExecuteUnopposed(action.gameObject);
                ClearCombatState(action.gameObject);
            }
        }
        UIToHideDuringCombat.SetActive(true);
        CombatEnd.Invoke();
    }

    private void ClearCombatState(GameObject unit)
    {
        if (unit == null) return;

        ActionComponent[] actions = unit.GetComponentsInChildren<ActionComponent>();

        foreach (ActionComponent action in actions)
        {
            SkillComponent skill = action.GetComponent<SkillComponent>();
            if (skill != null)
            {
                skill.ClearCurrentSkill();
                skill.SetNotClashing();
            }

            TargetComponent target = action.GetComponent<TargetComponent>();
            if (target != null)
            {
                target.ClearTargets();
            }
        }
    }
}