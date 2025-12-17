using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MagicCrate : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private new FirstPersonCamera camera;

    private bool isToggled = false;
    private bool toggle = false;

    private void Awake() => canvas.SetActive(false);
    public void OnFirstButtonClicked() => Toggle(true);
    public void OnSecondButtonClicked() => Toggle(true);
    public void Toggle(bool value) => StartCoroutine(SetUIMode(value));


    private IEnumerator SetUIMode(bool value)
    {
        yield return CoroutineHelper.WaitForEndOfFrame;

        canvas.SetActive(value);
        camera.SwitchCursorMode(value);
    }
}
