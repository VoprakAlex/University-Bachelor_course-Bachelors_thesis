using UnityEngine;
using UnityEngine.EventSystems;

public class TargetActionController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PlayerController _playerController;
    private TargetStatsView _targetStatsView;
    private ArcArrow2D _arcArrow;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _targetStatsView = FindAnyObjectByType<TargetStatsView>();
        _arcArrow = GetComponent<ArcArrow2D>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_playerController.CanChooseTarget)
            return;

        GameObject target = gameObject;

        // Назначаем цель игроку
        _playerController._targetComponent.SetMainTarget(target);

        // Компоненты игрока

        int playerSpeed =
            _playerController.PlayerObject.GetComponentInParent<SpeedComponent>().CurrentSpeed;

        // Компоненты цели
        int targetSpeed =
            target.GetComponentInParent<SpeedComponent>().CurrentSpeed;

        Debug.Log(targetSpeed);
        Debug.Log(playerSpeed);

        // Если цель медленнее игрока — начинается Clash
        if (targetSpeed < playerSpeed)
        {
            TargetComponent targetTarget =
                target.GetComponent<TargetComponent>();

            SkillComponent targetSkill =
                target.GetComponent<SkillComponent>();

            // Цель выбирает игрока своей целью
            targetTarget?.SetMainTarget(_playerController.PlayerObject.GetComponentInChildren<ActionComponent>().gameObject);

            // Clash у цели
            targetSkill?.SetClashing();

            // Clash у игрока
            _playerController._skillComponent.SetClashing();
        }

        UnitStatsView unitStatsView = FindAnyObjectByType<UnitStatsView>();
        if (unitStatsView != null)
        {
            unitStatsView.Hide();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetStatsView.Show(gameObject);
        _arcArrow?.ShowArc();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetStatsView.Hide();

        _arcArrow?.HideArc();
    }
}