#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

public class TabsManager : MonoBehaviour
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(TabsManager))]
    private class TabsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TabsManager tabsManager = (TabsManager)target;

            if (GUILayout.Button("Add Tab"))
            {
                GameObject @object = Instantiate(tabsManager.tabPrefab, tabsManager.transform);
                @object.GetComponent<Tab>().Init(tabsManager.container);
            }
        }
    }
    #endif
    
    [SerializeField] private GameObject tabPrefab;
    [SerializeField] private ObjectsContainer container;

    public void CreateTab(List<StoredObjectData> storedObjectsData, string name)
    {
        GameObject tabObject = Instantiate(tabPrefab, transform);
        tabObject.GetComponent<Tab>().Init(container, storedObjectsData, name);
    }
}