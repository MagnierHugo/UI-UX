#pragma warning disable IDE0090


using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UI;

using TMPro;


public sealed class Stove : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactCanvas;
    private Button[] buttons;
    [SerializeField] private Transform[] placedItemPositions;
    private Ingredient[] placedItems;
    private readonly List<CookingProgress> cookingProgresses = new List<CookingProgress>();
    [SerializeField] private Slider intensitySlider;
    [SerializeField] private TextMeshProUGUI temperatureText;
    private void Awake()
    {
        interactCanvas.SetActive(false);

        buttons = interactCanvas.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(OnEndInteract);
        buttons[0].onClick.AddListener(PlaceLeftHandContent);
        buttons[1].onClick.AddListener(OnEndInteract);
        buttons[1].onClick.AddListener(PlaceRightHandContent);

        intensitySlider.onValueChanged.AddListener(UpdateTemperatureText);
        placedItems = new Ingredient[placedItemPositions.Length];
        temperatureText.text = $"{0.0f:0.00}�C";
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(OnEndInteract);
        buttons[0].onClick.RemoveListener(PlaceLeftHandContent);
        buttons[1].onClick.RemoveListener(OnEndInteract);
        buttons[1].onClick.RemoveListener(PlaceRightHandContent);

        intensitySlider.onValueChanged.RemoveListener(UpdateTemperatureText);
    }

    private void UpdateTemperatureText(float temperature)
        => temperatureText.text = $"{temperature:0.00}�C";

    private PlayerInteract playerInteract;
    private PlayerInventory playerInventory;
    public bool OnBeginInteract(PlayerInteract playerInteract_)
    {
        playerInteract = playerInteract_;
        playerInventory = playerInteract_.GetComponent<PlayerInventory>();

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(playerInventory.Hands[i]?.IsCookable ?? false);

        interactCanvas.SetActive(true);
        return true;
    }

    private void Update()
    {
        if (playerInventory == null)
            return;

        for (int i = 0; i < placedItems.Length; i++)
        {
            Ingredient current = placedItems[i];
            if (current != null)
                if (playerInventory.LeftHand == current || playerInventory.RightHand == current)
                    placedItems[i] = null;

        }

        for (int i = cookingProgresses.Count - 1; i >= 0 ; i--)
        {
            CookingProgress cookingProgress = cookingProgresses[i];
            int indexInPlacedItemList = cookingProgress.IndexInPlacedItems;
            Ingredient relevantItem = placedItems[indexInPlacedItemList];
            if (!ReferenceEquals(relevantItem, cookingProgress.TargetItem))
            {
                cookingProgresses.RemoveAt(i);
                continue;
            }

            if (cookingProgress.UpdateProgress(intensitySlider.value))
            {
                placedItems[indexInPlacedItemList] = Instantiate(relevantItem.ItemReturnedWhenCooked);
                placedItems[indexInPlacedItemList].transform.position = placedItemPositions[i].position;
                Destroy(relevantItem.gameObject);
                cookingProgresses.RemoveAt(i);
            }
        }

        if (!interactCanvas.activeSelf)
            return;

        
        interactCanvas.transform.LookAt(
            interactCanvas.transform.position + (interactCanvas.transform.position - playerInteract.transform.position)
        );
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
            if (placedItems[i] != null)
                continue;

            playerInventory.Hands[handIndex].transform.position = placedItemPositions[i].position + Vector3.up * (playerInventory.Hands[handIndex].BoxCollider.size.y / 2); // place it onto the stove
            placedItems[i] = playerInventory.Hands[handIndex];
            playerInventory.Hands[handIndex] = null;
            wasPlaced = true;
            cookingProgresses.Add(new CookingProgress(placedItems[i], i));
            break;
        }

        if (!wasPlaced)
            throw new Exception("Not placed: no spot available");
    }


    private class CookingProgress
    {
        public const int Target = 3;
        public float Current;
        public Ingredient TargetItem;
        public int IndexInPlacedItems;
        public CookingProgress(Ingredient item, int index)
        {
            TargetItem = item;
            IndexInPlacedItems = index;
        }
        public bool UpdateProgress(float progressValue)
        {
            Current += progressValue * Time.deltaTime;

            return Current >= Target;
        }

    }
}
