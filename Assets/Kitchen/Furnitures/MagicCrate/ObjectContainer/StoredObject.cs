using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoredObject : MonoBehaviour, IPointerDownHandler
{
    private ObjectsContainer container;
    public StoredObjectData Data { get; private set; }
    private new TextMeshProUGUI name;
    private TextMeshProUGUI description;
    [SerializeField] private Image image;

    public void Init(ObjectsContainer container, StoredObjectData storedObjectData, TextMeshProUGUI name, TextMeshProUGUI description)
    {
        this.container = container;
        Data = storedObjectData;
        image.sprite = Data.Sprite;
        this.name = name;
        this.description = description;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        name.text = Data.Name;
        description.text = Data.Description;
        container.SelectedObject = this;
    }

    public GameObject GetIngredient() => Data.IngredientPrefab;
}