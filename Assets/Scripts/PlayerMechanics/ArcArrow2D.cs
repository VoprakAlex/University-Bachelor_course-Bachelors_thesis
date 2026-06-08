using UnityEngine;

[RequireComponent(typeof(ActionComponent))]
[RequireComponent(typeof(TargetComponent))]
public class ArcArrow2D : MonoBehaviour
{
    private TargetComponent _targetComponent;

    [Header("Visuals")]
    [SerializeField] private LineRenderer lr;

    [Header("Arc")]
    [Range(10, 50)]
    [SerializeField] private int segments = 20;

    [SerializeField] private float arcHeight = 2f;

    private void Awake()
    {
        _targetComponent = GetComponent<TargetComponent>();

        if (lr == null)
            lr = GetComponent<LineRenderer>();

        lr.enabled = false;
    }

    public void ShowArc()
    {
        if (_targetComponent == null || _targetComponent.MainTarget == null)
        {
            HideArc();
            return;
        }

        lr.enabled = true;
        Draw();
    }

    public void HideArc()
    {
        lr.enabled = false;
        lr.positionCount = 0;
    }

    private void Draw()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = _targetComponent.MainTarget.transform.position;

        DrawArc(startPos, endPos);
    }

    private void DrawArc(Vector3 p0, Vector3 p2)
    {
        Vector3 mid = (p0 + p2) * 0.5f;

        Vector3 dir = (p2 - p0).normalized;
        Vector3 perp = Vector3.Cross(dir, Vector3.forward);

        Vector3 up = Vector3.up;
        Vector3 p1 = mid + up * arcHeight;

        lr.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;

            Vector3 pos =
                Mathf.Pow(1 - t, 2) * p0 +
                2 * (1 - t) * t * p1 +
                Mathf.Pow(t, 2) * p2;

            lr.SetPosition(i, pos);
        }
    }
}