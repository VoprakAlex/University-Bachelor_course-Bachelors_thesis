using UnityEngine;
using UnityEngine.EventSystems;

public class TargetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public TargetStatsView _targetStatsView;
    public PlayerController _playerController;

    [SerializeField] private float _hoverScale = 1.1f;

    private RectTransform _rectTransform;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();

        _targetStatsView = FindAnyObjectByType<TargetStatsView>();
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_playerController.CanChooseTarget)
            return;

        _playerController._targetComponent.SetMainTarget(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _defaultScale * _hoverScale;
        _targetStatsView.Show(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _defaultScale;
        _targetStatsView.Hide();
    }
}