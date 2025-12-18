using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.CompilerServices;

using System.Diagnostics.CodeAnalysis;

using UnityEngine;
using TMPro;

public sealed class DropDownOptionSelector : MonoBehaviour
{
	private TMP_Dropdown dropDown;
    private void Awake()
    {
        dropDown = GetComponent<TMP_Dropdown>();
        dropDown.options.Clear();
        foreach (string value in Enum.GetNames(typeof(ItemType)))
            dropDown.options.Add(
                new TMP_Dropdown.OptionData(value)
            );
    }
}
