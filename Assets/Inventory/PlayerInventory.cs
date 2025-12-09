using UnityEngine;
using UnityEngine.UI;

public sealed class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Vector3[] renderPositions = new Vector3[2];
    private readonly GameObject[] inventory = new GameObject[2];
    private readonly Item[] hands = new Item[2];
    private Item LeftHand
    {
        get => hands[0];
        set => hands[0] = value;
    }
    private Item RightHand
    {
        get => hands[1];
        set => hands[1] = value;
    }
    [SerializeField] private Image[] handsPreview;
    private Image LeftHandPreview
    {
        get => handsPreview[0];
        set => handsPreview[0] = value;
    }
    private Image RightHandPreview
    {
        get => handsPreview[1];
        set => handsPreview[1] = value;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        renderPositions = renderPositions.Length switch
        {
            0 => new Vector3[2] { default, default },
            1 => new Vector3[2] { renderPositions[0], default },
            _ => new Vector3[2] { renderPositions[0], renderPositions[1] }
        };

        handsPreview = handsPreview.Length switch
        {
            0 => new Image[2],
            1 => new Image[2] { handsPreview[0], null },
            _ => new Image[2] { handsPreview[0], handsPreview[1] }
        };
    }
#endif

    public void PickupInLeftHand(Item item) => PickupItem(item, 0);
    public void PickupInRightHand(Item item) => PickupItem(item, 1);
    public void PickupItem(Item item, int handIndex)
    {
        if (hands[handIndex] != null) // sth is already held in this hand
        {
            hands[handIndex].transform.position = item.transform.position; // place the previously held item at the newly held item position
            hands[handIndex].gameObject.SetActive(true);
        }

        hands[handIndex] = item;
        handsPreview[handIndex].sprite = item.InventoryPreview;
        item.gameObject.SetActive(false);
    }

    public void PickupItem(Item item, Hand hand)
    {
        if (item == null)
            return;

        int index = (int)hand;
        GameObject itemInHand = inventory[index];
        if (itemInHand != null)
        {
            itemInHand.transform.position = item.transform.position;
        }

        inventory[index] = item.Prefab;
        item.transform.position = renderPositions[index];
    }

    private void Update()
    {
        DebugOSD.Display(nameof(LeftHand), LeftHand);
        DebugOSD.Display(nameof(RightHand), RightHand);
    }
}

public enum Hand
{
    Left,
    Right
}
