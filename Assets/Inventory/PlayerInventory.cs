using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Vector3[] renderPositions = new Vector3[2];
    private readonly GameObject[] inventory = new GameObject[2] { null, null };

#if UNITY_EDITOR
    private void OnValidate()
    {
        renderPositions = renderPositions.Length switch
        {
            0 => new Vector3[2] { default, default },
            1 => new Vector3[2] { renderPositions[0], default },
            _ => new Vector3[2] { renderPositions[0], renderPositions[1] }
        };
    }
#endif

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
}

public enum Hand
{
    Left,
    Right
}
