using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoredObject : MonoBehaviour, IPointerDownHandler
{
    private StoredObjectData data;
    private new TextMeshProUGUI name;
    private TextMeshProUGUI description;
    [SerializeField] private Image image;

    public void Init(StoredObjectData storedObjectData, TextMeshProUGUI name, TextMeshProUGUI description)
    {
        data = storedObjectData;
        image.sprite = data.Sprite;
        this.name = name;
        this.description = description;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        name.text = data.Name;
        description.text = data.Description;
    }
}