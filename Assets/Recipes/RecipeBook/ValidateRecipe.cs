#pragma warning disable IDE0090

using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif


public sealed class ValidateRecipe : MonoBehaviour
{
    [SerializeField] private RectTransform verticalLayoutGroup;
    [SerializeField] private RecipesSO recipes;
    [SerializeField] private TMP_Dropdown recipeOutputDropDown;
    [SerializeField] private TMP_InputField recipeName;
    [SerializeField] private RecipeBookPagesHandler recipeBookPagesHandler;


    private Button button;
    private void Awake() => (button = GetComponent<Button>()).onClick.AddListener(TryBuildRecipe);
    private void OnDestroy() => button.onClick.RemoveListener(TryBuildRecipe);

    private void Update() => button.interactable = verticalLayoutGroup.childCount > 2; // at least 2 ingredients + validateButton

    private void TryBuildRecipe()
    {
        Dictionary<IngredientType, int> ingredientOccurenceCount = new Dictionary<IngredientType, int>();
        for (int i = 0; i < verticalLayoutGroup.childCount - 1; i++)
        {
            TMP_Dropdown currentDropDown = verticalLayoutGroup.GetChild(i).GetComponent<TMP_Dropdown>();
            IngredientType currentIngredient = (IngredientType)Enum.Parse(
                typeof(IngredientType),
                currentDropDown.options[currentDropDown.value].text
            );

            if (!ingredientOccurenceCount.ContainsKey(currentIngredient))
                ingredientOccurenceCount[currentIngredient] = 0;
            ingredientOccurenceCount[currentIngredient]++;
        }

        bool allRecipeUnique = true;
        foreach (RecipeSO recipe in recipes.Recipes)
            if (!(allRecipeUnique &= !recipe.RecipeIsSame(ingredientOccurenceCount)))
                break;

        if (allRecipeUnique)
        {
            RecipeSO @new = RecipeSO.CreateFromIngredientOccurenceMap(
                    ingredientOccurenceCount,
                    (IngredientType)Enum.Parse(typeof(IngredientType), recipeOutputDropDown.options[recipeOutputDropDown.value].text),
                    recipes
                );

            recipeBookPagesHandler.AddPage(@new);
#if UNITY_EDITOR && false
            AssetDatabase.CreateAsset(@new, $"Assets/Recipes/{recipeName.text}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            recipes.Recipes.Add(@new);

        }
    }
}
