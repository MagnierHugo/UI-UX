using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public sealed class PlayerInteract : MonoBehaviour
{
	[SerializeField] private new Camera camera;
	[SerializeField] private float interactRange;
	[SerializeField] private Transform pickUpCanvas;
    private InputAction interactAction;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    private PlayerMovement playerMovement;
    [SerializeField] private Material glintMaterial;
    private Material initialMaterial;
    [SerializeField] private Button firstButton;
    [SerializeField] private Button secondButton;
    private event Action OnFirstButtonClicked;
    private void FirstButtonClicked()
    {
        OnFirstButtonClicked?.Invoke();
        ReleaseActionLock();
    }
    private event Action OnSecondButtonClicked;
    private void SecondButtonClicked()
    {
        OnSecondButtonClicked?.Invoke();
        ReleaseActionLock();
    }

    private PlayerInventory playerInventory;
    private void Awake()
    {
        InputActionMap inputActionMap = GetComponent<PlayerInput>().currentActionMap;
        interactAction = inputActionMap.FindAction("Interact", true);
        pickUpCanvas.gameObject.SetActive(false);
        playerInventory = GetComponent<PlayerInventory>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        interactAction.Enable();
        interactAction.started += OnInteracted;

        firstButton.onClick.AddListener(FirstButtonClicked);
        secondButton.onClick.AddListener(SecondButtonClicked);
    }
    private void OnDisable()
    {
        interactAction.started -= OnInteracted;
        interactAction.Disable();

        firstButton.onClick.RemoveListener(FirstButtonClicked);
        secondButton.onClick.RemoveListener(SecondButtonClicked);
    }
    private bool interacted;
    private void OnInteracted(InputAction.CallbackContext context) => interacted = true;

    private GameObject highlightedGameObject;
    private GameObject interactedGameObject;
    [SerializeField] private float verticalOffsetForCanvas;

    
    private void Update()
    {                                                                                                            
        //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camera.transform.position, camera.transform.forward * interactRange, Color.green);
        bool raycastHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactRange, Layers.Interactable, QueryTriggerInteraction.Ignore);
        if (raycastHit && hit.collider.TryGetComponent<IInteractable>(out var interactable))
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
                OnFirstButtonClicked += interactable.OnFirstButtonClicked;
                OnSecondButtonClicked += interactable.OnSecondButtonClicked;
            }
        }
        else if (highlightedGameObject != null)
        {
            // strip highlighting effect
            highlightedGameObject.GetComponent<MeshRenderer>().sharedMaterial = initialMaterial;
            highlightedGameObject = null;
        }

        if (interactedGameObject != null)
        {
            if (!pickUpCanvas.gameObject.activeSelf)
            {
                pickUpCanvas.gameObject.SetActive(true);
                firstPersonCamera.SwitchCursorMode(true);
                playerMovement.enabled = false;
            }
            pickUpCanvas.transform.LookAt(transform);
            if (Input.GetKey(KeyCode.Escape))
            {
                interactable = interactedGameObject.GetComponent<IInteractable>();
                OnFirstButtonClicked -= interactable.OnFirstButtonClicked;
                OnSecondButtonClicked -= interactable.OnSecondButtonClicked;
                interactedGameObject = null;
            }
        }
        else
        {
            if (pickUpCanvas.gameObject.activeSelf)
            {
                pickUpCanvas.gameObject.SetActive(false);
                firstPersonCamera.SwitchCursorMode(false);
                playerMovement.enabled = true;
            }
        }

        interacted = false;
    }

    private void ReleaseActionLock()
    {
        highlightedGameObject.GetComponent<MeshRenderer>().sharedMaterial = initialMaterial;
        highlightedGameObject = null;

        IInteractable interactable = interactedGameObject.GetComponent<IInteractable>();
        OnFirstButtonClicked -= interactable.OnFirstButtonClicked;
        OnSecondButtonClicked -= interactable.OnSecondButtonClicked;
        interactedGameObject = null;

        pickUpCanvas.gameObject.SetActive(false);
        firstPersonCamera.SwitchCursorMode(false);
        playerMovement.enabled = true;
    }
}

public interface IInteractable
{
    public void OnFirstButtonClicked();
    public void OnSecondButtonClicked();
}