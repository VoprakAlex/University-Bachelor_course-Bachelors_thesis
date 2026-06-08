using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class DynamicSpacing : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] public HorizontalLayoutGroup LayoutGroup;

    [Header("Container")]
    [SerializeField] public RectTransform Container;

    [Header("Settings")]
    [SerializeField] public int MaxCards = 24;

    private void Awake()
    {
        if (LayoutGroup == null)
            LayoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (Container == null)
            Container = GetComponent<RectTransform>();
    }

    public void RefreshSpacing()
    {
        int CardCount = transform.childCount;

        if (CardCount <= 6)
        {
            LayoutGroup.spacing = 0;
            return;
        }

        RectTransform FirstCard = transform.GetChild(0).GetComponent<RectTransform>();

        if (FirstCard == null)
            return;

        float AvailableWidth = Container.rect.width - FirstCard.rect.width * CardCount;

        LayoutGroup.spacing = AvailableWidth / (CardCount - 1);
    }
}