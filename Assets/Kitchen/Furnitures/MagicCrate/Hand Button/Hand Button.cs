using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hoverColor;

    private void OnValidate()
    {
        if (!image)
            return;

        image.color = baseColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print($"{transform.parent.gameObject.name}.{nameof(HandButton)}.{nameof(OnPointerDown)}");
    }

    public void OnPointerEnter(PointerEventData eventData) => image.color = hoverColor;
    public void OnPointerExit(PointerEventData eventData) => image.color = baseColor;
}
