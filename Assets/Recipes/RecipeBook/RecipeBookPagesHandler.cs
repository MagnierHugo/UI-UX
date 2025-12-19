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
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RecipesSO recipes;
    private int PagesCount => pagesRoot.childCount;
    private int currentPage = 0;

    private readonly Button[] buttons = new Button[2];
    private readonly List<Button> summaryButtons = new List<Button>();
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

        foreach (var button in summaryButtons)
            button.onClick.RemoveAllListeners();
    }

    private void InitPages()
    {
        foreach (RecipeSO recipe in recipes.Recipes)
            AddPage(recipe, false);
        AddSummaryPage();
    }

    private void AddSummaryPage()
    {
        Transform @new = Instantiate(pagePrefab, pagesRoot);
        for (int i = 0; i < recipes.Recipes.Count; i++)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, @new);
            buttonGO.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = recipes.Recipes[i].RecipeName;
            int index = i + 1;
            buttonGO.GetComponent<Button>().onClick.AddListener(() => JumpToPage(index));
        }

        @new.gameObject.SetActive(false);
    }

    public void AddPage(RecipeSO recipe, bool duringGameplay = true)
    {
        if (duringGameplay)
            Destroy(pagesRoot.GetChild(pagesRoot.childCount - 1).gameObject); // kill summary page to refresh it easily when called durting gameplay

        Transform @new = Instantiate(pagePrefab, pagesRoot);
        TextMeshProUGUI textMeshPro = Instantiate(pageItemPrefab, @new).GetComponent<TextMeshProUGUI>();
        textMeshPro.color = Color.aquamarine;
        textMeshPro.text = recipe.RecipeName;
        textMeshPro.fontSize = 48;
        foreach (IngredientType itemType in recipe.Ingredients)
            Instantiate(pageItemPrefab, @new).GetComponent<TextMeshProUGUI>().text = itemType.ToString();
        textMeshPro = Instantiate(pageItemPrefab, @new).GetComponent<TextMeshProUGUI>();
        textMeshPro.color = Color.green;
        textMeshPro.text = recipe.Output.IngredientType.ToString();
        @new.gameObject.SetActive(false);

        if (duringGameplay)
            AddSummaryPage();
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

    private void JumpToPage(int index)
    {
        pagesRoot.GetChild(currentPage).gameObject.SetActive(false);
        currentPage = index;
        pagesRoot.GetChild(currentPage).gameObject.SetActive(true);
    }
}
