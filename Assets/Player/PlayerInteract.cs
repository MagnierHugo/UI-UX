using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public sealed class PlayerInteract : MonoBehaviour
{
	[SerializeField] private new Camera camera;
	[SerializeField] private float interactRange;
	[SerializeField] private Transform pickUpCanvas;
    private InputAction interactAction;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    [SerializeField] private Material glintMaterial;
    private Material initialMaterial;
    private void Awake()
    {
        InputActionMap inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        interactAction = inputActionMap.FindAction("Interact", true);
        pickUpCanvas.gameObject.SetActive(false);
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

    private GameObject highlightedGameObject;
    private GameObject interactedGameObject;
    [SerializeField] private float verticalOffsetForCanvas;
    private void Update()
    {                                                                                                            
        //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camera.transform.position, camera.transform.forward * interactRange, Color.green);
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactRange, Layers.Interactable, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (highlightedGameObject == null || highlightedGameObject != hit.collider.gameObject)
                {
                    highlightedGameObject = hit.collider.gameObject;
                    // actually apply highlighting effect
                    MeshRenderer meshRenderer = highlightedGameObject.GetComponent<MeshRenderer>();
                    initialMaterial = meshRenderer.material;
                    meshRenderer.sharedMaterial = glintMaterial;
                }

                if (interacted)
                {
                    interactedGameObject = hit.collider.gameObject;
                    pickUpCanvas.transform.position = hit.collider.bounds.center + Vector3.up * verticalOffsetForCanvas;
                }
            }
            else if (highlightedGameObject != null)
            {
                // strip highlighting effect
                highlightedGameObject.GetComponent<MeshRenderer>().sharedMaterial = initialMaterial;
                highlightedGameObject = null;
                print("1");
            }
        }
        else if (highlightedGameObject != null)
        {
            // strip highlighting effect
            print("2");
            highlightedGameObject.GetComponent<MeshRenderer>().sharedMaterial = initialMaterial;
            highlightedGameObject = null;
        }

        if (interactedGameObject != null)
        {
            if (!pickUpCanvas.gameObject.activeSelf)
            {
                pickUpCanvas.gameObject.SetActive(true);
                firstPersonCamera.SwitchCursorMode(true);
            }
            pickUpCanvas.transform.LookAt(transform);
            static bool Happened()
            {
                print(nameof(Happened));
                return true;
            }
            if (Happened() && Input.GetKey(KeyCode.Escape))
            {
                interactedGameObject = null;
                print("been through");
            }
        }
        else
        {
            if (pickUpCanvas.gameObject.activeSelf)
            {
                pickUpCanvas.gameObject.SetActive(false);
                firstPersonCamera.SwitchCursorMode(false);
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



#if false


                    yyyjjjjjjyuuiijjllll
#endif