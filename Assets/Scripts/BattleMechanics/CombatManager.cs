using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    [Header("Handlers")]
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
        SkillComponent attackerSkillComponent = Attacker.GetComponent<SkillComponent>();
        Skill attackerSkill = attackerSkillComponent.CurrentSkill;

        if (attackerSkillComponent.IsClashing)
        {
            _clashHandler.ExecuteClash(Attacker);
        }
        else
        {
            _unopposedHandler.ExecuteUnopposed(Attacker);
        }
        CombatEnd.Invoke();
    }
}