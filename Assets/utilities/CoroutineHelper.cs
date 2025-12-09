using System.Runtime.CompilerServices;

using UnityEngine;

public sealed class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper instance;
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private void Awake() => instance = this;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HandleCoroutine(System.Collections.IEnumerator coroutine) => instance.StartCoroutine(coroutine);
}
