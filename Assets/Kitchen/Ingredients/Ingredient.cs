using UnityEngine;


public class Ingredient : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    //[field: SerializeField] public Sprite InventoryPreview { get; private set; }
    [SerializeField] private PlayerInventory playerInventory;
    [field: SerializeField, HideInInspector] public float MeshHeight { get; private set; }

    private void OnValidate()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshHeight = meshRenderer.bounds.extents.y;
    }

    public void OnFirstButtonClicked() => playerInventory.PickupInLeftHand(this);

    public void OnSecondButtonClicked() => playerInventory.PickupInRightHand(this);
}
