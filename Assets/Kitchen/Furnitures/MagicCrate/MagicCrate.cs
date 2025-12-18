using System.Collections;
using System.Collections.Generic;
using EditorUtilities;
using UnityEngine;

public class MagicCrate : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private new FirstPersonCamera camera;
    [SerializeField] private Transform tabs;

    [Space]
    [Header("Magic")]
    [SerializeField] private bool isMagic;
    [SerializeField, If(nameof(isMagic))] private int tabMinNumber;
    [SerializeField, If(nameof(isMagic))] private int tabMaxNumber;
    [SerializeField, If(nameof(isMagic))] private int minObjectsPerTab;
    [SerializeField, If(nameof(isMagic))] private int maxObjectsPerTab;
    [SerializeField, If(nameof(isMagic))] private List<StoredObjectData> availableObjects;


    private void Awake() => canvas.SetActive(false);
    public void OnFirstButtonClicked() => Toggle(true);
    public void OnSecondButtonClicked() => Toggle(true);
    public void Toggle(bool value) => StartCoroutine(SetUIMode(value));


    private IEnumerator SetUIMode(bool value)
    {
        yield return CoroutineHelper.WaitForEndOfFrame;

        canvas.SetActive(value);
        camera.SwitchCursorMode(value);

        if (!value)
            yield break;

        if (isMagic)
            CreateTabs();

        tabs.GetChild(0).GetChild(0).GetComponent<TabButton>().Select();
    }

    private void CreateTabs()
    {
        TabsManager tabsManager = tabs.GetComponent<TabsManager>();
        int tabsNumber = Random.Range(tabMinNumber, tabMaxNumber);
        for (int i = 0; i < tabsNumber; i++)
        {
            int objectsNumber = Random.Range(minObjectsPerTab, maxObjectsPerTab);
            List<StoredObjectData> storedObjectDatas = new List<StoredObjectData>(objectsNumber);
            for (int j = 0; j < objectsNumber; j++)
                storedObjectDatas.Add(availableObjects[Random.Range(0, availableObjects.Count)]);

            tabsManager.CreateTab(storedObjectDatas, $"Tab n°{i}");
        }
    }
}
