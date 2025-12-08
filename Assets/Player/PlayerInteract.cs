using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public sealed class PlayerInteract : MonoBehaviour
{
	[SerializeField] private new Camera camera;
	[SerializeField] private float interactRange;
    private InputAction interactAction;
    private void Awake()
    {
        InputActionMap inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        interactAction = inputActionMap.FindAction("Interact", true);
    }
    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.started += OnInteracted;
    }
    private void OnDisable()
    {
        interactAction.started -= OnInteracted;
        interactAction.Disable();
    }
    private bool interacted;
    private void OnInteracted(InputAction.CallbackContext context)
        => interacted = true;

    private void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.green);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, Layers.Interactable, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Hover();
                if (interacted)
                    interactable.Interact();
            }
        }
    }
    private void LateUpdate()
    {
        interacted = false;
    }
}

public interface IInteractable
{
    public void Hover();
    public void Interact();
}
