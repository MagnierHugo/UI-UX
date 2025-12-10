using UnityEngine;

public class Worktop : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient placedItem;
    [SerializeField] private PlayerInventory playerInventory;

    //private Vector3 itemPosition;

    private void OnValidate()
    {
        if (placedItem == null)
            return;

        Instantiate(placedItem.Prefab, transform.position, Quaternion.identity, transform);
    }

    public void OnFirstButtonClicked()
        => placedItem = placedItem != null ? playerInventory.PickupInLeftHand(placedItem) : playerInventory.PlaceItemInLeftHand();

    public void OnSecondButtonClicked()
        => placedItem = placedItem != null ? playerInventory.PickupInRightHand(placedItem) : playerInventory.PlaceItemInRightHand();
}
