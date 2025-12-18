using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using UnityEngine.UI;


public sealed class AddIngredientToList : MonoBehaviour
{
	[SerializeField] private GameObject dropDownPrefab;
    [SerializeField] private RectTransform verticalLayoutGroup;

	private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AddIngredientDropDown);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(AddIngredientDropDown);
    }

    private void AddIngredientDropDown()
    {
        /*GameObject @new = */Instantiate(dropDownPrefab, verticalLayoutGroup);
        //@new.GetComponent<RectTransform>()   
    }
}
