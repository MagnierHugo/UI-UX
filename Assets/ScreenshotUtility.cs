#pragma warning disable IDE0090
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

using UnityEditor;

using UnityEngine;


public sealed class ScreenshotUtility : EditorWindow
{
    public int resolution = 1024;

    [MenuItem("Tools/ScreenshotUtility")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotUtility>("ScreenshotUtility");
    }
    string savePath;
    private void OnGUI()
    {
        if (GUILayout.Button("Take Screenshot"))
            Capture(savePath);

        savePath = EditorGUILayout.TextField("target save path", savePath);
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
        RenderTexture rt = new RenderTexture(resolution, resolution, 24);
        cam.targetTexture = rt;

        Texture2D tex = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);

        cam.Render();
        RenderTexture.active = rt;

        tex.ReadPixels(new Rect(0, 0, resolution, resolution), 0, 0);
        tex.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        if (Application.isPlaying)
            Destroy(rt);
        else
            DestroyImmediate(rt);

        byte[] imageData = tex.EncodeToPNG();

        System.IO.File.WriteAllBytes(path, imageData); // Unity requires a file on disk first
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
