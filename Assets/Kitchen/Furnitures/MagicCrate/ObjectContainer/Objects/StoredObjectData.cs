using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewStoredObject", menuName = "Scriptable Objects/Stored Object")]
public class StoredObjectData : ScriptableObject
{
    [field: SerializeField] public GameObject IngredientPrefab { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
}
