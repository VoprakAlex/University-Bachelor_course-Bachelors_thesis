using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float HoverScale = 1.1f;

    private Vector3 DefaultScale;

    private void Awake()
    {
        DefaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = DefaultScale * HoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = DefaultScale;
    }
}