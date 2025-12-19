using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnityEditor.UI;

using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Scriptable Objects/Recipes")]
public sealed class RecipesSO : ScriptableObject
{
    [field: SerializeField] public List<RecipeSO> Recipes { get; private set; }
    [field: SerializeField] public List<ItemToPrefabMap> ItemToPrefabMap { get; private set; } // dictionarys aren t serializable by default and can t be bothered doing it myself

    public void SanitizeRecipeList()
    {       
        for (int i = Recipes.Count - 1; i >= 0; i--)
            if (Recipes[i] == null)
                Recipes.RemoveAt(i);
    }
    public Item GetRelevantItemFromItemType(ItemType itemType)
    {
        for (int i = 0; i < ItemToPrefabMap.Count; i++)
            if (ItemToPrefabMap[i].ItemType == itemType)
                return ItemToPrefabMap[i].ItemPrefab;

        return null;
    }
}

[Serializable]
public struct ItemToPrefabMap
{
    public ItemType ItemType;
    public Item ItemPrefab;
}