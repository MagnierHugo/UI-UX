using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class Item : MonoBehaviour
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField, HideInInspector] public Sprite ItemSprite { get; private set; }

#if UNITY_EDITOR
    [HideInInspector] public bool AwaitingPreview = false;

    private void OnValidate()
    {
        if (Prefab == null) return;

        // Mark for preview generation
        AwaitingPreview = true;
        print($"{gameObject.name}: {nameof(OnValidate)}");
    }

    public void SetPreviewSprite(Sprite sprite)
    {
        ItemSprite = sprite;
        AwaitingPreview = false;
        print($"{gameObject.name}: {nameof(SetPreviewSprite)}");

        EditorUtility.SetDirty(this);
    }
#endif
}
