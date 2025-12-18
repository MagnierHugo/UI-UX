using System;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [SerializeField] private Animator fridgeDoorAnimator;
    [SerializeField] private Animator freezerDoorAnimator;
    [SerializeField] private Animator crisperDoorAnimator;

    private bool isFridgeOpen = false;
    private bool isFreezerOpen = false;
    private bool isCrisperOpen = false;

    public void ToggleFridgeDoor()
    {
        if (isFridgeOpen)
        {
            if (isFreezerOpen) ToggleFreezerDoor();
            if (isCrisperOpen) ToggleCrisperDoor();
        }

        isFridgeOpen = !isFridgeOpen;
        fridgeDoorAnimator.SetBool("isOpen", isFridgeOpen);
    }

    public void ToggleFreezerDoor()
    {
        isFreezerOpen = !isFreezerOpen;
        freezerDoorAnimator.SetBool("isOpen", isFreezerOpen);
    }

    public void ToggleCrisperDoor()
    {
        isCrisperOpen = !isCrisperOpen;
        crisperDoorAnimator.SetBool("isOpen", isCrisperOpen);
    }
}
