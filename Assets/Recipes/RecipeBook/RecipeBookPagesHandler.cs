using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


public sealed class RecipeBookPagesHandler : MonoBehaviour
{
    [SerializeField] private Transform pagesRoot;
    [SerializeField] private Transform pagePrefab;
    [SerializeField] private GameObject pageItemPrefab;
    [SerializeField] private RecipesSO recipes;
    private int PagesCount => pagesRoot.childCount;
    private int currentPage = 0;

    private readonly Button[] buttons = new Button[2];
    private void Awake()
    {
        InitPages();

        (buttons[0] = transform.GetChild(0).GetComponent<Button>()).onClick.AddListener(NextPage);
        (buttons[1] = transform.GetChild(1).GetComponent<Button>()).onClick.AddListener(PreviousPage);
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(NextPage);
        buttons[1].onClick.RemoveListener(PreviousPage);
    }

    private void InitPages()
    {
        foreach (RecipeSO recipe in recipes.Recipes)
            AddPage(recipe);
    }

    public void AddPage(RecipeSO recipe)
    {
        Transform @new = Instantiate(pagePrefab, pagesRoot);
        foreach (ItemType itemType in recipe.Ingredients)
            Instantiate(pageItemPrefab, @new).GetComponent<TextMeshProUGUI>().text = itemType.ToString();
        TextMeshProUGUI textMeshPro = Instantiate(pageItemPrefab, @new).GetComponent<TextMeshProUGUI>();
        textMeshPro.color = Color.green;
        textMeshPro.text = recipe.Output.Itemtype.ToString();
        @new.gameObject.SetActive(false);
    }

    private void NextPage()
    {
        pagesRoot.GetChild(currentPage).gameObject.SetActive(false);
        currentPage = ++currentPage % PagesCount;
        pagesRoot.GetChild(currentPage).gameObject.SetActive(true);
    }
    private void PreviousPage()
    {
        pagesRoot.GetChild(currentPage).gameObject.SetActive(false);
        currentPage = --currentPage % PagesCount;
        if (currentPage < 0)
            currentPage += PagesCount;
        pagesRoot.GetChild(currentPage).gameObject.SetActive(true);
    }
}
