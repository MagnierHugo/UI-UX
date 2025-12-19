using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using UnityEngine.UI;


public sealed class RemoveIngredientFromList : MonoBehaviour
{
    [SerializeField] private RectTransform verticalLayoutGroup;

    private Button button;
    private void Awake() => (button = GetComponent<Button>()).onClick.AddListener(RemoveIngredientDropDown);
    private void OnDestroy() => button.onClick.RemoveListener(RemoveIngredientDropDown);
    private void RemoveIngredientDropDown() => Destroy(verticalLayoutGroup.GetChild(verticalLayoutGroup.childCount - 2).gameObject);
    private void Update() => button.interactable = verticalLayoutGroup.childCount > 1;
}
