#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class ItemPreviewGenerator
{
    static ItemPreviewGenerator()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        var items = Object.FindObjectsByType<Item>(FindObjectsSortMode.None);

        foreach (var item in items)
        {
            if (!item.AwaitingPreview || item.Prefab == null)
                continue;

            Texture2D tex = AssetPreview.GetAssetPreview(item.Prefab);

            if (tex == null)
            {
                // Preview not ready yet, try again next update
                continue;
            }

            // Convert texture -> sprite
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );

            item.SetPreviewSprite(sprite);
        }
    }
}
#endif

