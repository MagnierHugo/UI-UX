using UnityEngine;
using UnityEngine.Events;

public class RefrigeratorDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onDoorClicked;

    public bool OnBeginInteract(PlayerInteract playerInteract)
    {
        onDoorClicked?.Invoke();
        return false;
    }

    public void OnEndInteract() { }
}
