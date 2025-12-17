using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Tab tab;
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!tab.SelectTab())
            return;

        image.color = tab.SelectedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!tab.OnPointerEnter())
            return;

        image.color = tab.SelectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!tab.OnPointerExit())
            return;

        image.color = baseColor;
    }
}
