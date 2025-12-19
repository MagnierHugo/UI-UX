using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;


public sealed class PlayerRecipeBook : MonoBehaviour
{
	[SerializeField] private GameObject recipeBook;
	[SerializeField] private FirstPersonCamera firstPersonCamera;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private RecipesSO recipesSO;

    private void Start() => recipesSO.SanitizeRecipeList();
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

        recipeBook.SetActive(!recipeBook.activeSelf);
        firstPersonCamera.SwitchCursorMode(recipeBook.activeSelf);
        playerMovement.enabled = !recipeBook.activeSelf;
    }
}
