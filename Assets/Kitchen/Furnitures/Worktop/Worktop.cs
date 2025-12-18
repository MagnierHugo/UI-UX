using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Worktop : MonoBehaviour, IInteractable
{
    [SerializeField] private Ingredient placedIngredient;
    [SerializeField] private GameObject interactCanvasPrefab;

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
        if (placedIngredient == null)
            return;

        GameObject ingredient = Instantiate(
            placedIngredient.Prefab,
            transform.position + new Vector3(0f, transform.localScale.y / 2 + placedIngredient.MeshHeight, 0f),
            Quaternion.identity
        );

        placedIngredient = ingredient.GetComponent<Ingredient>();

        ingredient.transform.parent = transform;
    }

    private PlayerInteract playerInteract;
    private PlayerInventory playerInventory;
    private GameObject interactCanvasInstance;
    
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        interactCanvasInstance = Instantiate(interactCanvasPrefab, transform.position + new Vector3(0f, transform.localScale.y / 2 + .3f, 0f), Quaternion.identity);
        interactCanvasInstance.transform.LookAt(interactCanvasInstance.transform.position + (interactCanvasInstance.transform.position - playerInteract.transform.position));
        Button[] buttons = interactCanvasInstance.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(PickupInLeftHand);
        buttons[0].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(PickupInRightHand);
        buttons[1].onClick.AddListener(OnEndInteract);
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

    private void PickupInLeftHand()
    {
        if (placedIngredient != null)
            placedIngredient.transform.parent = null;

        placedIngredient = placedIngredient != null ? playerInventory.PickupInLeftHand(placedIngredient) : playerInventory.PlaceItemInLeftHand();
        if (placedIngredient == null)
            return;

        placedIngredient.transform.position = transform.position + new Vector3(0f, transform.localScale.y / 2 + placedIngredient.MeshHeight, 0f);
        placedIngredient.transform.parent = transform;
    }

    private void PickupInRightHand()
    {
        if (placedIngredient != null)
            placedIngredient.transform.parent = null;

        placedIngredient = placedIngredient != null ? playerInventory.PickupInRightHand(placedIngredient) : playerInventory.PlaceItemInRightHand();
        if (placedIngredient == null)
            return;

        placedIngredient.transform.position = transform.position + new Vector3(0f, transform.localScale.y / 2 + placedIngredient.MeshHeight, 0f);
        placedIngredient.transform.parent = transform;
    }
}
