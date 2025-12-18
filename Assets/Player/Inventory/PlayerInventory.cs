using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class PlayerInventory : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] private Transform[] refCubes = new Transform[2];
#endif

    [SerializeField, HideInInspector] private Vector3[] renderPositions = new Vector3[2];
    private readonly Ingredient[] hands = new Ingredient[2];

    private Ingredient LeftHand
    {
        get => hands[0];
        set => hands[0] = value;
    }
    private Ingredient RightHand
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
        if (refCubes == null)
            return;

        refCubes = refCubes.Length switch
        {
            0 => new Transform[2] { null, null },
            1 => new Transform[2] { refCubes[0], null },
            _ => new Transform[2] { refCubes[0], refCubes[1] }
        };

        renderPositions[0] = refCubes[0] ? refCubes[0].position : Vector3.zero;
        renderPositions[1] = refCubes[1] ? refCubes[1].position : Vector3.zero;

        //handsPreview = handsPreview.Length switch
        //{
        //    0 => new Image[2],
        //    1 => new Image[2] { handsPreview[0], null },
        //    _ => new Image[2] { handsPreview[0], handsPreview[1] }
        //};
    }
#endif

    public Ingredient PickupInLeftHand(Ingredient ingredient, bool swapItems = true) => PickupItem(ingredient, 0, swapItems);
    public Ingredient PickupInRightHand(Ingredient ingredient, bool swapItems = true) => PickupItem(ingredient, 1, swapItems);
    private Ingredient PickupItem(Ingredient ingredient, int handIndex, bool swapItems)
    {
        if (ingredient == null) 
            return null;

        Ingredient ingredientInHand = hands[handIndex];
        if (ingredientInHand != null) // sth is already held in this hand
        {
            if (!swapItems)
                return ingredient;

            ingredientInHand.transform.position = ingredient.transform.position; // place the previously held item at the newly held item position
            //itemInHand.gameObject.SetActive(true);
        }

        hands[handIndex] = ingredient;
        ingredient.transform.position = renderPositions[handIndex];
        //handsPreview[handIndex].sprite = item.InventoryPreview;
        //item.gameObject.SetActive(false);
        return ingredientInHand;
    }

    public bool PickupAndInstanciateInLeftHand(GameObject ingredientPrefab) => PickupAndInstanciateItem(ingredientPrefab, 0);
    public bool PickupAndInstanciateInRightHand(GameObject ingredientPrefab) => PickupAndInstanciateItem(ingredientPrefab, 1);
    private bool PickupAndInstanciateItem(GameObject ingredientPrefab, int handIndex)
    {
        print(nameof(PickupAndInstanciateItem));
        if (ingredientPrefab == null) 
            return false;

        Ingredient ingredientInHand = hands[handIndex];
        if (ingredientInHand != null)
            return false;

        Ingredient ingredient = Instantiate(ingredientPrefab).GetComponent<Ingredient>();
        hands[handIndex] = ingredient;
        ingredient.transform.position = renderPositions[handIndex];
        return true;
    }

    public Ingredient PlaceItemInLeftHand() => PlaceItem(0);
    public Ingredient PlaceItemInRightHand() => PlaceItem(1);
    private Ingredient PlaceItem(int handIndex)
    {
        Ingredient ingredientInHand = hands[handIndex];
        if (ingredientInHand == null)
            return null;

        hands[handIndex] = null;
        return ingredientInHand;
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
