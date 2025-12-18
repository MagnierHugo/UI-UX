using UnityEngine;
using UnityEngine.Events;

public class RefrigeratorDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onDoorClicked;

    public void OnFirstButtonClicked() => onDoorClicked?.Invoke();
    public void OnSecondButtonClicked() => onDoorClicked.Invoke();
}
