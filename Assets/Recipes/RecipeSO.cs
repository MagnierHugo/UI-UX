#pragma warning disable IDE0090

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using TMPro;

using Unity.VisualScripting.FullSerializer;

using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public sealed class RecipeSO : ScriptableObject
{
    [field: SerializeField] public List<ItemType> Ingredients { get; private set; }
    [field: SerializeField] public Item Output { get; private set; }

    private Dictionary<ItemType, int> ingredientOccurenceCount;
    public Dictionary<ItemType, int> IngredientOccurenceCount => ingredientOccurenceCount ??= BuildIngredientOccurenceCount();
    private Dictionary<ItemType, int> BuildIngredientOccurenceCount()
    {
        Dictionary<ItemType, int> toReturn = new Dictionary<ItemType, int>();

        foreach (ItemType currentIngredient in Ingredients)
        { 
            if (!toReturn.ContainsKey(currentIngredient))
                toReturn[currentIngredient] = 0;
            toReturn[currentIngredient]++;
        }

        return toReturn;
    }


    public bool TryMake(List<Item> inputIngredients, [NotNullWhen(true)] out Item result)
    {
        result = null;
        List<Item> consumedItems = new List<Item>();
        foreach (ItemType ingredient in Ingredients)
        {
            bool foundOne = false;
            for (int i = 0; i < inputIngredients.Count; i++)
            {
                if (inputIngredients[i] == null)
                    continue;

                if (inputIngredients[i].Itemtype == ingredient)
                {
                    consumedItems.Add(inputIngredients[i]);
                    inputIngredients.Remove(inputIngredients[i]);
                    foundOne = true;
                    break;
                }
            }

            if (!foundOne)
                return false;
        }

        foreach (GameObject ingredientGameObject in consumedItems.Select(single => single.gameObject))
            Destroy(ingredientGameObject);

        result = Instantiate(Output);
        return true;
    }

    public bool RecipeIsSame(Dictionary<ItemType, int> otherRecipeIngredientOccurenceCount)
    {
        if (otherRecipeIngredientOccurenceCount.Count != IngredientOccurenceCount.Count) // not the same amount of ingredient so must be different
            return false;
        
        foreach (KeyValuePair<ItemType, int> keyValuePair in IngredientOccurenceCount)
        {
            bool bothHaveIngredient = otherRecipeIngredientOccurenceCount.TryGetValue(keyValuePair.Key, out int ingredientCount);

            if (!bothHaveIngredient || ingredientCount != keyValuePair.Value) // different count for that ingerdient
                return false;
        }

        return true;
    }

    public static RecipeSO CreateFromIngredientOccurenceMap(Dictionary<ItemType, int> ingredientOccurenceCount_, ItemType recipeOutput, RecipesSO recipes)
    {
        RecipeSO @new = CreateInstance<RecipeSO>();
        @new.ingredientOccurenceCount = ingredientOccurenceCount_;
        @new.Ingredients = new List<ItemType>();
        foreach (KeyValuePair<ItemType, int> keyValuePair in ingredientOccurenceCount_)
            for (int i = 0; i < keyValuePair.Value; i++)
                @new.Ingredients.Add(keyValuePair.Key);

        @new.Output  = recipes.GetRelevantItemFromItemType(recipeOutput);
        return @new;
    }

}
