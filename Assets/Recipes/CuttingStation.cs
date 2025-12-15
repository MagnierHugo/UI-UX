using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public sealed class CuttingStation : MonoBehaviour, IInteractable
{
    private PlayerInventory playerInventory;
    [SerializeField] private GameObject interactCanvas;

    private void Awake()
    {
        interactCanvas.SetActive(false);

        Button[] buttons = interactCanvas.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnEndInteract);    
        buttons[0].onClick.AddListener(ChopLeftHandContent);    
        buttons[1].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(ChopRightHandContent);    
    }

    private void ChopLeftHandContent() => ChopHandContent(0);
    private void ChopRightHandContent() => ChopHandContent(1);
    private void ChopHandContent(int handIndex)
    {
        if (playerInventory.Hands[handIndex] == null)
            return;


    }

    private PlayerInteract playerInteract;
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        interactCanvas.SetActive(true);
        return true;
    }

    public void OnEndInteract()
    {
        playerInteract.EndInteraction();
        playerInteract = null;

        interactCanvas.SetActive(false);
    }
}
