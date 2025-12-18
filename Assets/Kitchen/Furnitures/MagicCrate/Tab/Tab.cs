using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    private static Tab selectedTab;

    [Header("Appearance")]
    [SerializeField] private Image image;
    [SerializeField] private Color baseColor;
    [field: SerializeField] public Color SelectedColor { get; private set; }
    [SerializeField] private TextMeshProUGUI tabName;
    [SerializeField] private new string name;

    [Space]
    [Header("Content")]
    [SerializeField] private List<StoredObjectData> storedObjects;
    private bool isSelected = false;

    [Space]
    [SerializeField] private ObjectsContainer container;

    private void OnValidate()
    {
        if (image)
            image.color = isSelected ? SelectedColor : baseColor;

        if (tabName != null)
            tabName.text = name;
    }

    private void Awake()
    {
        if (!isSelected)
            return;

        if (selectedTab != null)
            selectedTab.ResetTab();
        
        selectedTab = this;
    }

    public void Init(ObjectsContainer container, List<StoredObjectData> storedObjectDatas = null, string name = null)
    {
        this.container = container;

        if (storedObjectDatas != null)
            storedObjects = storedObjectDatas;

        if (!String.IsNullOrEmpty(name))
            this.name = name;
    }

    public bool SelectTab()
    {
        if (isSelected)
            return false;

        isSelected = true;
        selectedTab?.ResetTab();
        selectedTab = this;
        container.OnTabSelected(storedObjects);
        return true;
    }

    public bool OnPointerEnter()
    {
        if (isSelected)
            return false;

        image.color = SelectedColor;
        return true;
    }

    public bool OnPointerExit()
    {
        if (isSelected)
            return false;

        image.color = baseColor;
        return true;
    }

    private void ResetTab()
    {
        image.color = baseColor;
        isSelected = false;
    }
}