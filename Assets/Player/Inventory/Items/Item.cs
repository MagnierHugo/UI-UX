#pragma warning disable IDE0090


using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(BoxCollider))]
public sealed class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public ItemType Itemtype { get; private set; }
    
    [SerializeField] private GameObject pickupCanvasPrefab;
    private GameObject pickupCanvasInstance;
    private readonly Vector3 canvasHeightOffset = new Vector3(0, .3f, 0f);
    [field: SerializeField] public Vector3 PreviewOrientation { get; private set; }

    public BoxCollider BoxCollider { get; private set; } 
    private void Awake() => BoxCollider = GetComponent<BoxCollider>();

    [Header("Chop")]
    [field: SerializeField] public bool IsChoppable { get; private set; }
    [field: SerializeField] public List<Item> ItemsReturnedWhenChopped { get; private set; }


    [Header("Cook")]
    [field: SerializeField] public bool IsCookable { get; private set; }
    [field: SerializeField] public Item ItemReturnedWhenCooked { get; private set; }

    private void PickUpInLeftHand() => playerInventory.PickupInLeftHand(this);
    private void PickUpInRightHand() => playerInventory.PickupInRightHand(this);

    private void Update()
    {
        if (playerInteract == null)
            return;

        pickupCanvasInstance.transform.LookAt(pickupCanvasInstance.transform.position + (pickupCanvasInstance.transform.position - playerInteract.transform.position));
    }

    private PlayerInventory playerInventory;
    private PlayerInteract playerInteract;
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        pickupCanvasInstance = Instantiate(pickupCanvasPrefab, BoxCollider.bounds.center + canvasHeightOffset, Quaternion.identity);
        Button[] buttons = pickupCanvasInstance.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(PickUpInLeftHand);
        buttons[0].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(PickUpInRightHand);
        buttons[1].onClick.AddListener(OnEndInteract);

        return true;
    }

    public void OnEndInteract()
    {
        playerInteract.EndInteraction();
        playerInteract = null;
        
        Button[] buttons = pickupCanvasInstance.GetComponentsInChildren<Button>();
        buttons[0].onClick.RemoveListener(PickUpInLeftHand);
        buttons[0].onClick.RemoveListener(OnEndInteract);
        buttons[1].onClick.RemoveListener(PickUpInRightHand);
        buttons[1].onClick.RemoveListener(OnEndInteract);

        Destroy(pickupCanvasInstance);
    }
}

public enum ItemType
{
    Aubergine,
    BurgerBun,
    BurgerBunBottom,
    BurgerBunTop,
    Carrot,
    Tomato,
    TomatoChopped,
    Lettuce,
    LettuceChopped,
    Cheddar,
    CheddarSliced,
    Fish,
    Beef,
    BeefCooked,
    Burger,
}