#pragma warning disable IDE0090
#if UNITY_EDITOR
using System.IO;

using UnityEditor;

using UnityEngine;


public sealed class ScreenshotUtility : EditorWindow
{
    [MenuItem("Tools/ScreenshotUtility")]
    public static void ShowWindow() => GetWindow<ScreenshotUtility>("ScreenshotUtility");

    private RenderTexture previewRenderTexture;
    private string savePath;
    private int resolutionX = 1024;
    private int resolutionY = 1024;
    bool showPreview;
    

    private void OnGUI()
    {
        savePath = EditorGUILayout.TextField("Target save path", savePath);
        GUILayout.Label(nameof(resolutionX));
        resolutionX = EditorGUILayout.IntField(resolutionX);
        GUILayout.Label(nameof(resolutionY));
        resolutionY = EditorGUILayout.IntField(resolutionY);

        if (GUILayout.Button("Take Screenshot"))
            Capture(savePath);

        showPreview = GUILayout.Toggle(showPreview, "Show Preview");

        if (showPreview)
        {
            if (previewRenderTexture == null || previewRenderTexture.width != resolutionX || previewRenderTexture.height != resolutionY)
                previewRenderTexture = new RenderTexture(resolutionX, resolutionY, 16);

            SceneView view = SceneView.lastActiveSceneView;
            if (view)
            {
                view.camera.targetTexture = previewRenderTexture;
                view.camera.Render();
                view.camera.targetTexture = null;

                GUILayout.Label(previewRenderTexture, GUILayout.Width(200), GUILayout.Height(200 * resolutionY / resolutionX));
            }
        }       
    }

    private void Capture(string path)  
    {
        path = Path.Combine("Assets", path);
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null)
        {
            Debug.LogWarning("No active SceneView found!");
            return;
        }

        Camera cam = sceneView.camera;
        RenderTexture rt = new RenderTexture(resolutionX, resolutionY, 24);
        cam.targetTexture = rt;

        Texture2D tex = new Texture2D(resolutionX, resolutionY, TextureFormat.RGB24, false);

        cam.Render();
        RenderTexture.active = rt;

        tex.ReadPixels(new Rect(0, 0, resolutionX, resolutionY), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        if (Application.isPlaying)
            Destroy(rt);
        else
            DestroyImmediate(rt);

        byte[] imageData = tex.EncodeToPNG();

        File.WriteAllBytes(path, imageData); // Unity requires a file on disk first
        AssetDatabase.ImportAsset(path); // Import into project
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Default;
            importer.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        Debug.Log($"SceneView screenshot saved as asset: {path}");
    }
}

#endif