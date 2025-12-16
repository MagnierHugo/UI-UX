#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TabsManager : MonoBehaviour
{
    [CustomEditor(typeof(TabsManager))]
    private class TabsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TabsManager tabsManager = (TabsManager)target;

            if (GUILayout.Button("Add Tab"))
            {
                Instantiate(tabsManager.tabPrefab, tabsManager.transform);
            }
        }
    }
    
    [SerializeField] private GameObject tabPrefab;
}
#endif