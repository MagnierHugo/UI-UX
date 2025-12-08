using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Image[] hands = new Image[2] { null, null };

    private readonly GameObject[] inventory = new GameObject[2] { null, null };

    private void OnValidate()
    {
        hands = hands.Length switch
        {
            0 => new Image[2] { null, null },
            1 => new Image[2] { hands[0], null },
            _ => new Image[2] { hands[0], hands[1] }
        };
    }

    public void PickupItem(Item item, Hand hand)
    {
        if (item == null)
            return;

        int index = (int)hand;
        GameObject itemInHand = inventory[index];
        if (itemInHand != null)
            Instantiate(itemInHand, item.transform.position, Quaternion.identity);

        inventory[index] = item.Prefab;
        hands[index].sprite = item.ItemSprite;
    }
}

public enum Hand
{
    Left,
    Right
}
