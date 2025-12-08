using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class OnClick : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private UnityEvent<Item, Hand> OnClickEvent;

    private void OnMouseDown()
    {
        print($"{gameObject.name}: {nameof(OnMouseDown)}");

        if (Input.GetMouseButtonDown(0))
            OnClickEvent?.Invoke(item, Hand.Left);
        else if (Input.GetMouseButtonDown(1))
            OnClickEvent?.Invoke(item, Hand.Right);
    }
}
