using UnityEngine;
using UnityEngine.UI;

public class ActionStateView : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TargetComponent _targetComponent;

    [Header("Renderer")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private Sprite ReadySprite;

    private void Awake()
    {
        if (_targetComponent == null)
            _targetComponent = GetComponent<TargetComponent>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _targetComponent.OnMainTargetSet.AddListener(OnTargetChanged);
        _targetComponent.OnMainTargetCleared.AddListener(UpdateState);
    }

    private void OnDisable()
    {
        _targetComponent.OnMainTargetSet.RemoveListener(OnTargetChanged);
        _targetComponent.OnMainTargetCleared.RemoveListener(UpdateState);
    }

    private void OnTargetChanged(GameObject target)
    {
        UpdateState();
    }

    private void UpdateState()
    { 
        bool hasTarget = _targetComponent.MainTarget != null;

        if (!hasTarget)
        {
            _spriteRenderer.sprite = EmptySprite;
            return;
        }

        _spriteRenderer.sprite = ReadySprite;
    }
}