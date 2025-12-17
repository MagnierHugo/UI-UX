using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public sealed class CuttingStation : MonoBehaviour, IInteractable
{
    private PlayerInventory playerInventory;
    [SerializeField] private GameObject interactCanvas;
    [SerializeField] private Transform placedItemPosition;
    private Button[] buttons;

    private void Awake()
    {
        interactCanvas.SetActive(false);

        buttons = interactCanvas.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnEndInteract);    
        buttons[0].onClick.AddListener(ChopLeftHandContent);    
        buttons[1].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(ChopRightHandContent);    
    }

    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(OnEndInteract);
        buttons[0].onClick.RemoveListener(ChopLeftHandContent);
        buttons[1].onClick.RemoveListener(OnEndInteract);
        buttons[1].onClick.RemoveListener(ChopRightHandContent);
    }

    private void ChopLeftHandContent() => ChopHandContent(0);
    private void ChopRightHandContent() => ChopHandContent(1);
    private void ChopHandContent(int handIndex)
    {
        List<Item> outcome = playerInventory.Hands[handIndex].ItemsReturnedWhenChopped;
        Destroy(playerInventory.Hands[handIndex].gameObject);
        playerInventory.PickupItem(
            Instantiate(outcome[0]),
            handIndex
        );

        for (int i = 1; i < outcome.Count; i++)
        {
            // no work lmao
            _ = Physics.Raycast(placedItemPosition.position + Vector3.up * 20, Vector3.down, out var hit, 25, Layers.Interactable, QueryTriggerInteraction.Ignore);
            Item @new = Instantiate(outcome[i]);
            @new.transform.position = hit.point + Vector3.up * (@new.BoxCollider.size.y / 2); // place it onto the table or stack it onto items already on it
        }
    }

    private PlayerInteract playerInteract;
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        bool atLeastOneButtonEnabled = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            bool thisButtonEnabled = playerInventory.Hands[i]?.IsChoppable ?? false;
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
}
