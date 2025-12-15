using System.Runtime.CompilerServices;

using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Transform[] refCubes = new Transform[2];
#endif

    [SerializeField, HideInInspector] private Vector3[] renderPositions = new Vector3[2];
    public readonly Item[] Hands = new Item[2];

    public Item LeftHand
    {
        get => Hands[0];
        private set => Hands[0] = value;
    }
    public Item RightHand
    {
        get => Hands[1];
        private set => Hands[1] = value;
    }

#if UNITY_EDITOR && false
    private void OnValidate()
    {
        refCubes = refCubes.Length switch
        {
            0 => new Transform[2] { null, null },
            1 => new Transform[2] { refCubes[0], null },
            _ => new Transform[2] { refCubes[0], refCubes[1] }
        };

        renderPositions[0] = refCubes[0]?.position ?? Vector3.zero;
        renderPositions[1] = refCubes[1]?.position ?? Vector3.zero;

        //handsPreview = handsPreview.Length switch
        //{
        //    0 => new Image[2],
        //    1 => new Image[2] { handsPreview[0], null },
        //    _ => new Image[2] { handsPreview[0], handsPreview[1] }
        //};
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PickupInLeftHand(Item item) => PickupItem(item, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PickupInRightHand(Item item) => PickupItem(item, 1);
    public void PickupItem(Item item, int handIndex)
    {
        if (item == null) 
            return;

        Item itemInHand = Hands[handIndex];
        if (itemInHand != null) // sth is already held in this hand
        {
            itemInHand.transform.position = item.transform.position; // place the previously held item at the newly held item position
            //itemInHand.gameObject.SetActive(true);
        }

        Hands[handIndex] = item;
        item.transform.position = renderPositions[handIndex];
        //handsPreview[handIndex].sprite = item.InventoryPreview;
        //item.gameObject.SetActive(false);
    }
}

