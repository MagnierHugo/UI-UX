using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Spawner : MonoBehaviour, IInteractable
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField, HideInInspector] private Ingredient ingredient;

    private void OnValidate() => ingredient = ingredientPrefab.GetComponent<Ingredient>();

    private PlayerInteract playerInteract;
    private PlayerInventory playerInventory;
    private GameObject interactCanvasInstance;

    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        print("1");
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();
        print("2");

        interactCanvasInstance = ingredient.InstantiatePickupCanvas(boxCollider.bounds.center);
        interactCanvasInstance.transform.LookAt(interactCanvasInstance.transform.position + (interactCanvasInstance.transform.position - playerInteract.transform.position));
        Button[] buttons = interactCanvasInstance.GetComponentsInChildren<Button>();
        print("3");
        buttons[0].onClick.AddListener(PickupInLeftHand);
        buttons[0].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(PickupInRightHand);
        buttons[1].onClick.AddListener(OnEndInteract);
        print("4");
        return true;
    }

    public void OnEndInteract()
    {
        playerInteract.EndInteraction();
        playerInteract = null;
        
        Button[] buttons = interactCanvasInstance.GetComponentsInChildren<Button>();
        buttons[0].onClick.RemoveListener(PickupInLeftHand);
        buttons[0].onClick.RemoveListener(OnEndInteract);
        buttons[1].onClick.RemoveListener(PickupInRightHand);
        buttons[1].onClick.RemoveListener(OnEndInteract);

        Destroy(interactCanvasInstance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Ingredient SpawnIngredient()
        => Instantiate(ingredientPrefab).GetComponent<Ingredient>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PickupInLeftHand()
        => playerInventory.PickupInLeftHand(SpawnIngredient(), false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PickupInRightHand()
        => playerInventory.PickupInRightHand(SpawnIngredient(), false);
}
