using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static Tab selectedTab;

    [Header("Appearance")]
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;

    [Space]
    [Header("Content")]
    [SerializeField] private List<StoredObject> stockedObjects;
    [SerializeField] private bool isSelected = false;

    private void OnValidate()
    {
        if (!image)
            return;
        
        image.color = isSelected ? selectedColor : baseColor;
    }

    private void Awake()
    {
        if (!isSelected)
            return;

        if (selectedTab != null)
            selectedTab.ResetTab();
        
        selectedTab = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSelected)
            return;

        isSelected = true;
        selectedTab.ResetTab();
        selectedTab = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected)
            return;

        image.color = selectedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected)
            return;

        image.color = baseColor;
    }

    private void ResetTab()
    {
        image.color = baseColor;
        isSelected = false;
    }
}