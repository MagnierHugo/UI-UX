using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.UI;


public sealed class AddIngredientToList : MonoBehaviour
{
	[SerializeField] private GameObject dropDownPrefab;
    [SerializeField] private RectTransform verticalLayoutGroup;

	private Button button;
    private void Awake() => (button = GetComponent<Button>()).onClick.AddListener(AddIngredientDropDown);
    private void OnDestroy() => button.onClick.RemoveListener(AddIngredientDropDown);
    private void AddIngredientDropDown()
    {
        RectTransform validateButton = (RectTransform)verticalLayoutGroup.GetChild(verticalLayoutGroup.childCount - 1);
        validateButton.SetParent(null);

        Instantiate(dropDownPrefab, verticalLayoutGroup);
        validateButton.SetParent(verticalLayoutGroup); // keep it bottommost
        validateButton.anchoredPosition3D = new Vector3(
            validateButton.anchoredPosition3D.x,
            validateButton.anchoredPosition3D.y,
            0
        );
    }
}
