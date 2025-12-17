#if UNITY_EDITOR
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
                GameObject @object = Instantiate(tabsManager.tabPrefab, tabsManager.transform);
                @object.GetComponent<Tab>().Init(tabsManager.container);
            }
        }
    }
    
    [SerializeField] private GameObject tabPrefab;
    [SerializeField] private ObjectsContainer container;
}
#endif