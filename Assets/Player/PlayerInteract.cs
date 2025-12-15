using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public sealed class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRange;
    private InputAction interactAction;

    private PlayerMovement playerMovement;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    [SerializeField, HideInInspector] private Transform firstPersonCameraTransform;

    [SerializeField] private Material glintMaterial;
    private Material initialMaterial;
    private void OnValidate() => firstPersonCameraTransform = firstPersonCamera.transform;

    private void Awake()
    {
        InputActionMap inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        interactAction = inputActionMap.FindAction("Interact", true);
        playerMovement = GetComponent<PlayerMovement>();
    }

    private GameObject highlightedGameObject;
    private GameObject interactedGameObject;

    private bool isLockedIntoInteraction;
    private void Update()
    {
        Debug.DrawRay(firstPersonCameraTransform.position, firstPersonCameraTransform.forward * interactRange, Color.green);
        bool raycastHit = Physics.Raycast(firstPersonCameraTransform.position, firstPersonCameraTransform.forward, out RaycastHit hit, interactRange, Layers.Interactable, QueryTriggerInteraction.Collide);
        if (raycastHit && hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            MeshRenderer meshRenderer;
            if (highlightedGameObject != hit.collider.gameObject)
            {
                if (highlightedGameObject != null)
                {
                    if (highlightedGameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
                        meshRenderer.sharedMaterial = initialMaterial;
                }

                highlightedGameObject = hit.collider.gameObject;

                // actually apply highlighting effect if applicable
                if (highlightedGameObject.TryGetComponent<MeshRenderer>(out meshRenderer))
                {
                    initialMaterial = meshRenderer.material;
                    meshRenderer.sharedMaterial = glintMaterial;
                }
            }

            if (interactAction.WasPressedThisFrame())
            {
                interactedGameObject = hit.collider.gameObject;
                if (interactable.OnBeginInteract(this))
                    LockMovements();
            }
        }
        else if (highlightedGameObject != null)
        {
            // strip highlighting effect if was applicable
            if (highlightedGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
                meshRenderer.sharedMaterial = initialMaterial;
            highlightedGameObject = null;
        }

        if (isLockedIntoInteraction && Input.GetKey(KeyCode.Escape))
            interactedGameObject.GetComponent<IInteractable>().OnEndInteract();
    }

    public void EndInteraction()
    {
        UnlockMovements();

        interactedGameObject = null;

        isLockedIntoInteraction = false;
    }
    private void LockMovements()
    {
        isLockedIntoInteraction = true;

        firstPersonCamera.SwitchCursorMode(true);
        playerMovement.enabled = false;
    }

    private void UnlockMovements()
    {
        firstPersonCamera.SwitchCursorMode(false);
        playerMovement.enabled = true;

        isLockedIntoInteraction = false;
    }

}

public interface IInteractable
{
    /// <summary>
    /// returns wether the interaction will lock player into it
    /// </summary>
    /// <param name="playerInteract"></param>
    /// <returns></returns>
    public bool OnBeginInteract(PlayerInteract playerInteract);
    public void OnEndInteract();

}