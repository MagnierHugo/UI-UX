using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hoverColor;

    [SerializeField] private ObjectsContainer container;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private bool isLeftHand;

    private void OnValidate()
    {
        if (!image)
            return;

        image.color = baseColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print(nameof(OnPointerDown));
        bool pickedUp;
        if (isLeftHand)
            pickedUp = playerInventory.PickupAndInstanciateInLeftHand(container.SelectedObject?.GetIngredient());
        else
            pickedUp = playerInventory.PickupAndInstanciateInRightHand(container.SelectedObject?.GetIngredient());

        if (pickedUp)
            container.RemoveSelectedObject();
    }

    public void OnPointerEnter(PointerEventData eventData) => image.color = hoverColor;
    public void OnPointerExit(PointerEventData eventData) => image.color = baseColor;
}
