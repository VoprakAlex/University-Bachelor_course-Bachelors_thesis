using UnityEngine;
using UnityEngine.EventSystems;

public class SkillController : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Refs")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private SkillView _skillView;

    [Header("Hover")]
    [SerializeField] private float _hoverScale = 1.1f;

    private RectTransform _rectTransform;
    private Vector3 _defaultScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _defaultScale = transform.localScale;

        if (_skillView == null)
            _skillView = GetComponent<SkillView>();

        _playerController = FindAnyObjectByType<PlayerController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = _defaultScale * _hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _defaultScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerController._skillComponent.SetCurrentSkill(_skillView.Data);
    }
}