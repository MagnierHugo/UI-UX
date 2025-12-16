using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Scriptable Objects/Recipes")]
public sealed class RecipesSO : ScriptableObject
{
    [field: SerializeField] public List<RecipeSO> Recipes { get; private set; }
}