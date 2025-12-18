using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectsContainer : MonoBehaviour
{
    [SerializeField] private GameObject storedObjectPrefab;
    private List<StoredObjectData> storedObjects;
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    
    [HideInInspector] public StoredObject SelectedObject;

    public void OnTabSelected(List<StoredObjectData> storedObjects)
    {
        this.storedObjects = storedObjects;
        ClearObjects();
        CreateObjects();
        name.text = "";
        description.text = "";
    }

    public void RemoveSelectedObject()
    {
        print(nameof(RemoveSelectedObject));
        storedObjects.Remove(SelectedObject.Data);
        Destroy(SelectedObject.gameObject);
        SelectedObject = null;
    }

    private void CreateObjects()
    {
        foreach (StoredObjectData storedObject in storedObjects)
        {
            GameObject @object = Instantiate(storedObjectPrefab, transform);
            @object.GetComponent<StoredObject>().Init(this, storedObject, name, description);
        }
    }

    private void ClearObjects()
    {
        foreach (GameObject child in transform.Cast<Transform>().Select(t => t.gameObject))
            Destroy(child);
    }
}
