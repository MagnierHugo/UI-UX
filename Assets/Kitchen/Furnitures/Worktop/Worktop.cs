using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class Worktop : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient placedItem;
    [SerializeField] private PlayerInventory playerInventory;

    private bool shouldDestroyChildren = false;

    //private Vector3 itemPosition;

    private void OnValidate()
    {
        if (transform.childCount > 0)
            shouldDestroyChildren = true;
        else
            SpawnIngredient();
    }

    private void Update()
    {
        if (!shouldDestroyChildren)
            return;

        foreach (GameObject child in transform.Cast<Transform>().Select(t => t.gameObject))
        {
            if (Application.isPlaying)
                Destroy(child);
            else
                DestroyImmediate(child);
        }

        SpawnIngredient();
        shouldDestroyChildren = false;
    }

    private void SpawnIngredient()
    {
        if (placedItem == null)
            return;

        GameObject ingredient = Instantiate(
            placedItem.Prefab,
            transform.position + new Vector3(0f, transform.localScale.y / 2 + placedItem.MeshHeight, 0f),
            Quaternion.identity
        );

        placedItem = ingredient.GetComponent<Ingredient>();

        ingredient.transform.parent = transform;
    }

    public void OnFirstButtonClicked()
    {
        if (placedItem != null)
            placedItem.transform.parent = null;

        placedItem = placedItem != null ? playerInventory.PickupInLeftHand(placedItem) : playerInventory.PlaceItemInLeftHand();
        if (placedItem == null)
            return;

        placedItem.transform.position = transform.position + new Vector3(0f, transform.localScale.y / 2 + placedItem.MeshHeight, 0f);
        placedItem.transform.parent = transform;
    }

    public void OnSecondButtonClicked()
    {
        if (placedItem != null)
            placedItem.transform.parent = null;

        placedItem = placedItem != null ? playerInventory.PickupInRightHand(placedItem) : playerInventory.PlaceItemInRightHand();
        if (placedItem == null)
            return;

        placedItem.transform.position = transform.position + new Vector3(0f, transform.localScale.y / 2 + placedItem.MeshHeight, 0f);
        placedItem.transform.parent = transform;
    }
}
