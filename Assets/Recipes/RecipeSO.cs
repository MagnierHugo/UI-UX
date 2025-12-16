using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnityEngine;


[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public sealed class RecipeSO : ScriptableObject
{
    [field: SerializeField] public List<ItemType> Ingredients { get; private set; }
    [field: SerializeField] public Item Output { get; private set; }

    public bool TryMake(List<Item> inputIngredients, [NotNullWhen(true)] out Item result)
    {
        result = null;
        List<Item> consumedItems = new List<Item>();
        foreach (ItemType ingredient in Ingredients)
        {
            for (int i = 0; i < inputIngredients.Count; i++)
            {
                if (inputIngredients[i].Itemtype == ingredient)
                {
                    consumedItems.Add(inputIngredients[i]);
                    inputIngredients.Remove(inputIngredients[i]);
                    break;
                }
                else
                    return false;
            }            
        }

        foreach (GameObject ingredientGameObject in consumedItems.Select(single => single.gameObject))
            Destroy(ingredientGameObject);

        result = Instantiate(Output);
        return true;
    }
}
