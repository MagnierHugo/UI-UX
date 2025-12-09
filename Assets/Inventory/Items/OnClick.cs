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

        OnClickEvent?.Invoke(item, Input.GetKey(KeyCode.LeftShift) ? Hand.Left : Hand.Right);
    }
}
