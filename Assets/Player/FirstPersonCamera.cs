using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public sealed class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // The GameObject whose rotation the camera will follow
    [SerializeField] private Transform headTransform;

    private float xRotation = 0f; // Current rotation around the x-axis
    private float yRotation = 0f; // Current rotation around the y-axis

    [SerializeField] private float xSensitivity = 3;
    [SerializeField] private float ySensitivity = 3;

    private InputAction lookAction;
    private InputAction cursorModeAction;
    private void Awake()
    {
        PlayerInput input = playerTransform.GetComponent<PlayerInput>();
        lookAction = input.currentActionMap.FindAction("Look");
        cursorModeAction = input.currentActionMap.FindAction("CursorMode");
        SwitchCursorMode(false);
    }

    private void OnEnable()
    {
        lookAction.Enable();
        lookAction.performed += OnPlayerLook;

        cursorModeAction.Enable();
        cursorModeAction.performed += EnableCursorMode;
        cursorModeAction.canceled += DisableCursorMode;
    }


    private void OnDisable()
    {
        lookAction.performed -= OnPlayerLook;
        lookAction.Disable();

        cursorModeAction.performed -= EnableCursorMode;
        cursorModeAction.canceled -= DisableCursorMode;
        cursorModeAction.Disable();
    }

    private void EnableCursorMode(InputAction.CallbackContext _)
        => SwitchCursorMode(true);
    private void DisableCursorMode(InputAction.CallbackContext _)
        => SwitchCursorMode(false);
    private bool isInCursorMode;
    private void SwitchCursorMode(bool towardCursorMode)
    {
        isInCursorMode = towardCursorMode;
        Cursor.visible = towardCursorMode;
        Cursor.lockState = towardCursorMode ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    private Vector2 lookInputVector;
    private void OnPlayerLook(InputAction.CallbackContext context)
        => lookInputVector = context.ReadValue<Vector2>();

    private void LateUpdate()
    {
        if (isInCursorMode)
            return;

        // around x-axis
        xRotation -= lookInputVector.y * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -89f, 89f); // Clamp in order to avoid doing a flip when looking up/down too intensely

        headTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // around y-axis
        yRotation += lookInputVector.x * xSensitivity;
        playerTransform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        lookInputVector = Vector2.zero;
    }
}

