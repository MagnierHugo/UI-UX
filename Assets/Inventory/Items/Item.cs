#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif

using UnityEngine;


public class Item : MonoBehaviour
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField, HideInInspector] public Sprite ItemSprite { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        print(nameof(OnValidate));
        StartCoroutine(GeneratePreview());
    }

    private IEnumerator GeneratePreview()
    {
        if (Prefab == null)
            yield break;

        Texture2D previewTexture = null;
        while (previewTexture == null)
        {
            previewTexture = AssetPreview.GetAssetPreview(Prefab);
            yield return null;
        }


        ItemSprite = Sprite.Create(
            previewTexture,
            new Rect(0, 0, previewTexture.width, previewTexture.height),
            new Vector2(.5f, .5f)
        );

        print("Preview Generated");
    }
#endif
}
