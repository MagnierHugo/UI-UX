using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnityEngine;


[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public sealed class RecipeSO : ScriptableObject
{
    [field: SerializeField] public List<IngredientType> Ingredients { get; private set; }
    [field: SerializeField] public Ingredient Output { get; private set; }

    public bool TryMake(List<Ingredient> inputIngredients, [NotNullWhen(true)] out Ingredient result)
    {
        result = null;
        List<Ingredient> consumedItems = new List<Ingredient>();
        foreach (IngredientType ingredient in Ingredients)
        {
            bool foundOne = false;
            for (int i = 0; i < inputIngredients.Count; i++)
            {
                if (inputIngredients[i] == null)
                    continue;

                if (inputIngredients[i].IngredientType == ingredient)
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
}
