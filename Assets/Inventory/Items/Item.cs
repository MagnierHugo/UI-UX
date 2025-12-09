using UnityEngine;


public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public Sprite InventoryPreview { get; private set; }
    [SerializeField] private PlayerInventory playerInventory;

    public void OnFirstButtonClicked() => playerInventory.PickupInLeftHand(this);

    public void OnSecondButtonClicked() => playerInventory.PickupInRightHand(this);
}
