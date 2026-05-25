using UnityEngine;
using UnityEngine.EventSystems;

public class TargetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private PlayerController _playerController;

    [SerializeField] private float _hoverScale = 1.1f;

    private RectTransform _rectTransform;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerController._targetComponent.SetMainTarget(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _defaultScale * _hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _defaultScale;
    }
}