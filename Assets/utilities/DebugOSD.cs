#pragma warning disable IDE0090
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using System.Threading.Tasks;


using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// add some debugLocation option ?
// or make the debug movable (with saved pos)
[DefaultExecutionOrder(int.MinValue)]
public sealed class DebugOSD : MonoBehaviour
{
    static DebugOSD()
    {
        GameObject gameObject = new GameObject(nameof(DebugOSD));
        instance = gameObject.AddComponent<DebugOSD>();
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.CreateScene(nameof(DebugOSD)));
    }
    private const string NullKey = nameof(NullKey);
    private const string NullValue = nameof(NullValue);
    private static readonly DebugOSD instance;

    private bool active;

    private readonly Dictionary<string, string> namedEntries = new Dictionary<string, string>();
    private readonly List<string> anonymousEntries = new List<string>();

    private readonly Dictionary<string, string> displayForTimeEntries = new Dictionary<string, string>();
    private readonly Dictionary<string, string> displayForTimeAnonymousEntries = new Dictionary<string, string>();
    private int displayForTimeAnonymousEntriesIndex;


    [Conditional("UNITY_EDITOR")]
    private void Update()
    {
        active = Input.GetKeyDown(KeyCode.F3) ? !active : active;

        namedEntries.Clear();
        anonymousEntries.Clear();
    }

    [Conditional("UNITY_EDITOR")]
    private void OnGUI()
    {
        if (!active)
            return;

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.Space(5);
        GUILayout.BeginVertical(GUILayout.Height(Screen.height));

        GUILayout.BeginVertical("box");

        if (namedEntries.Count + anonymousEntries.Count + displayForTimeEntries.Count + displayForTimeAnonymousEntries.Count == 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("No tracked value RN");
            GUILayout.EndHorizontal();
        }
        else
        {
            DisplayData(namedEntries);
            DisplayData(displayForTimeEntries);
            DisplayData(anonymousEntries);
            DisplayData(displayForTimeAnonymousEntries);
        }

        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)][Conditional("UNITY_EDITOR")]
    private void DisplayData(Dictionary<string, string> data)
    {
        foreach (KeyValuePair<string, string> entry in data)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{entry.Key}: {entry.Value}");
            GUILayout.EndHorizontal();
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)][Conditional("UNITY_EDITOR")]
    private void DisplayData(List<string> data)
    {
        foreach (string entry in data)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(entry);
            GUILayout.EndHorizontal();
        }
    }


    #region Plain Display
    [MethodImpl(MethodImplOptions.AggressiveInlining)][Conditional("UNITY_EDITOR")]
    public static void Display<T>(T value) => instance.anonymousEntries.Add(value?.ToString() ?? NullValue);
    [MethodImpl(MethodImplOptions.AggressiveInlining)][Conditional("UNITY_EDITOR")]
    public static void Display<TKey, TValue>(TKey key, TValue value) => instance.namedEntries[key?.ToString() ?? $"{NullKey}{Random.value}"] = value?.ToString() ?? NullValue;
    #endregion

    #region Display For Time
    [Conditional("UNITY_EDITOR")]
    public static async void DisplayForTime<T>(T value, int durationInMs)
    {
        string index = instance.displayForTimeAnonymousEntriesIndex++.ToString();
        instance.displayForTimeAnonymousEntries[index] = value?.ToString() ?? NullValue;
        ;

        await Task.Delay(durationInMs);

        _ = instance.displayForTimeAnonymousEntries.Remove(index);
    }
    [Conditional("UNITY_EDITOR")]
    public static async void DisplayForTime<TKey, TValue>(TKey key, TValue value, int durationInMs)
    {
        string keyAsStr = key?.ToString() ?? $"{NullKey}{Random.value}";
        instance.displayForTimeEntries[keyAsStr] = value?.ToString() ?? NullValue;

        await Task.Delay(durationInMs);

        _ = instance.displayForTimeEntries.Remove(keyAsStr);
    }


    public static class DisplayDurationTypesInMS
    {
        public const int JustForTheSakeOfit = 10;
        public const int ExtraBrief = 1500;
        public const int Brief = 2000;
        public const int Medium = 3000;
        public const int Long = 4000;
        public const int BurnITIntoMyScreen = int.MaxValue;
        public const int HauntMeForEternity = int.MaxValue;
    }

    #endregion
}