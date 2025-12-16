using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public sealed class RecipeMaker : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private RecipesSO recipes;
    private Button[] buttons;
    [SerializeField] private Transform[] placedItemPositions;
    private Item[] placedItems;
    private void Awake()
    {
        interactCanvas.SetActive(false);

        buttons = interactCanvas.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnEndInteract);
        buttons[0].onClick.AddListener(PlaceLeftHandContent);
        buttons[1].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(PlaceRightHandContent);

        placedItems = new Item[placedItemPositions.Length];
    }
    private PlayerInteract playerInteract;
    private PlayerInventory playerInventory;
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        bool atLeastOneButtonEnabled = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            bool thisButtonEnabled = playerInventory.Hands[i] != null;
            buttons[i].gameObject.SetActive(thisButtonEnabled);
            atLeastOneButtonEnabled |= thisButtonEnabled;
        }

        interactCanvas.SetActive(atLeastOneButtonEnabled);
        return atLeastOneButtonEnabled;
    }

    public void OnEndInteract()
    {
        playerInteract.EndInteraction();
        playerInteract = null;

        interactCanvas.SetActive(false);
    }

    private void PlaceLeftHandContent() => PlaceHandContent(0);
    private void PlaceRightHandContent() => PlaceHandContent(1);
    private void PlaceHandContent(int handIndex)
    {
        bool wasPlaced = false;
        for (int i = 0; i < placedItemPositions.Length; i++)
        {
            if (Physics.CheckSphere(placedItemPositions[i].position, .1f, Layers.Interactable, QueryTriggerInteraction.Ignore))
                continue;

            playerInventory.Hands[handIndex].transform.position = placedItemPositions[i].position + Vector3.up * (playerInventory.Hands[handIndex].BoxCollider.size.y / 2); // place it onto the table or stack it onto items already on it
            placedItems[i] = playerInventory.Hands[handIndex];
            playerInventory.Hands[handIndex] = null;
            wasPlaced = true;
            break;
        }

        if (!wasPlaced)
            throw new Exception("Not placed: no spot available");

        List<Item> itemsAsList = placedItems.ToList();
        bool success = false;
        Item result = null;
        foreach (var recipe in recipes.Recipes)
            if (success |= recipe.TryMake(itemsAsList, out result))
                break;

        if (success)
            playerInventory.PickupItem(result, handIndex);            
    }
}
