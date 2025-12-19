#pragma warning disable IDE0090

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;


using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public sealed class RecipeSO : ScriptableObject
{
    [field: SerializeField] public string RecipeName { get; private set; }
    [field: SerializeField] public List<IngredientType> Ingredients { get; private set; }
    [field: SerializeField] public Ingredient Output { get; private set; }

    private Dictionary<IngredientType, int> ingredientOccurenceCount;
    public Dictionary<IngredientType, int> IngredientOccurenceCount => ingredientOccurenceCount ??= BuildIngredientOccurenceCount();
    private Dictionary<IngredientType, int> BuildIngredientOccurenceCount()
    {
        Dictionary<IngredientType, int> toReturn = new Dictionary<IngredientType, int>();

        foreach (IngredientType currentIngredient in Ingredients)
        { 
            if (!toReturn.ContainsKey(currentIngredient))
                toReturn[currentIngredient] = 0;
            toReturn[currentIngredient]++;
        }

        return toReturn;
    }


    public bool TryMake(List<Ingredient> inputIngredients, [NotNullWhen(true)] out Ingredient result)
    {
        result = null;
        List<Ingredient> consumedItems = new List<Ingredient>();
        foreach (IngredientType recipeIngredientType in Ingredients)
        {
            bool foundOne = false;
            for (int i = 0; i < inputIngredients.Count; i++)
            {
                if (inputIngredients[i] == null)
                    continue;

                if (inputIngredients[i].IngredientType == recipeIngredientType)
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

    public bool RecipeIsSame(Dictionary<IngredientType, int> otherRecipeIngredientOccurenceCount)
    {
        if (otherRecipeIngredientOccurenceCount.Count != IngredientOccurenceCount.Count) // not the same amount of ingredient so must be different
            return false;
        
        foreach (KeyValuePair<IngredientType, int> keyValuePair in IngredientOccurenceCount)
        {
            bool bothHaveIngredient = otherRecipeIngredientOccurenceCount.TryGetValue(keyValuePair.Key, out int ingredientCount);

            if (!bothHaveIngredient || ingredientCount != keyValuePair.Value) // different count for that ingerdient
                return false;
        }

        return true;
    }

    public static RecipeSO CreateFromIngredientOccurenceMap(Dictionary<IngredientType, int> ingredientOccurenceCount_, IngredientType recipeOutput, string recipeName, RecipesSO recipes)
    {
        RecipeSO @new = CreateInstance<RecipeSO>();
        @new.ingredientOccurenceCount = ingredientOccurenceCount_;
        @new.Ingredients = new List<IngredientType>();
        foreach (KeyValuePair<IngredientType, int> keyValuePair in ingredientOccurenceCount_)
            for (int i = 0; i < keyValuePair.Value; i++)
                @new.Ingredients.Add(keyValuePair.Key);

        @new.Output  = recipes.GetRelevantItemFromItemType(recipeOutput);
        @new.RecipeName = recipeName;
        return @new;
    }

}
