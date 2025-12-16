using UnityEngine;

public class Spawner : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private PlayerInventory playerInventory;

    public void OnFirstButtonClicked()
        => playerInventory.PickupInLeftHand(SpawnIngredient(), false);

    public void OnSecondButtonClicked()
        => playerInventory.PickupInRightHand(SpawnIngredient(), false);

    private Ingredient SpawnIngredient()
        => Instantiate(ingredientPrefab).GetComponent<Ingredient>();
}
