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
    public Ingredient GetRelevantItemFromItemType(IngredientType itemType)
    {
        for (int i = 0; i < ItemToPrefabMap.Count; i++)
            if (ItemToPrefabMap[i].ItemType == itemType)
                return ItemToPrefabMap[i].ItemPrefab;

        return null;
    }

    public (RecipeSO recipe, int index) FindRecipeFromName(string name)
    {
        for (int i = 0; i < Recipes.Count;i++)
            if (Recipes[i].RecipeName == name)
                return (Recipes[i], i);

        return (null, -1);
    }
}

[Serializable]
public struct ItemToPrefabMap
{
    public IngredientType ItemType;
    public Ingredient ItemPrefab;
}