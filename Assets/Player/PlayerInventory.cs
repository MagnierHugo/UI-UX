using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] private Transform[] refCubes = new Transform[2];
#endif

    [SerializeField, HideInInspector] private Vector3[] renderPositions = new Vector3[2];
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

    //[SerializeField] private Image[] handsPreview;
    //private Image LeftHandPreview
    //{
    //    get => handsPreview[0];
    //    set => handsPreview[0] = value;
    //}
    //private Image RightHandPreview
    //{
    //    get => handsPreview[1];
    //    set => handsPreview[1] = value;
    //}

#if UNITY_EDITOR
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

    public void PickupInLeftHand(Item item) => PickupItem(item, 0);
    public void PickupInRightHand(Item item) => PickupItem(item, 1);
    public void PickupItem(Item item, int handIndex)
    {
        if (item == null) 
            return;

        Item itemInHand = hands[handIndex];
        if (itemInHand != null) // sth is already held in this hand
        {
            itemInHand.transform.position = item.transform.position; // place the previously held item at the newly held item position
            //itemInHand.gameObject.SetActive(true);
        }

        hands[handIndex] = item;
        item.transform.position = renderPositions[handIndex];
        //handsPreview[handIndex].sprite = item.InventoryPreview;
        //item.gameObject.SetActive(false);
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
