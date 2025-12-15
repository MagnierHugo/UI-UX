#if false
using System;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    [Serializable]
    private struct DoorData
    {
        public Transform doorCurrent;
        public Transform doorClosed;
        public Transform doorOpen;
        [HideInInspector] public bool isOpen;
        [HideInInspector] public bool isMoving;
    }

    [SerializeField] private DoorData door;
    [SerializeField] private DoorData freezerDoor;
    [SerializeField] private DoorData crisperDoor;

    private void Update()
    {
        UpdateFridgeDoor();
        UpdateFreezerDoor();
        UpdateCripsterDoor();
    }

    private void UpdateCripsterDoor()
    {
        if (!crisperDoor.isMoving)
            return;

        if (!crisperDoor.isOpen)
        {
            if (crisperDoor.doorOpen)
        }
    }

    private void UpdateFreezerDoor()
    {
        if (!isFreezerDoorMoving)
            return;
    }

    private void UpdateFridgeDoor()
    {
        if (!isFridgeDoorMoving)
            return;
    }

    public void ToggleFridgeDoor()
    {
        if (isFridgeOpen)
        {
            isFridgeOpen = false;
            isFreezerOpen = false;
            isCrisperOpen = false;
            return;
        }

        isFridgeOpen = true;
    }

    public void ToggleFreezerDoor() => isFreezerOpen = !isFreezerOpen;
    public void ToggleCrisperDoor() => isCrisperOpen = !isCrisperOpen;
} 
#endif
