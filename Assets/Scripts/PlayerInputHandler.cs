// 
// Copyright (c) 2023 Kim Hyun Deok
//
// PlayerInputHandler.cs
//
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Transform playerTransform;
    private Camera mainCamera;

    private void Awake()
    {
        playerTransform = transform;
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    public void Move(InputAction.CallbackContext context)
    {
        bool pressed = context.ReadValueAsButton();
        if (pressed)
        {
            // TODO: Use Rays from the camera
            // https://docs.unity3d.com/Manual/CameraRays.html

            Vector3 mousePosition = new Vector3(Mouse.current.position.x.ReadValue(),
                Mouse.current.position.y.ReadValue(), 10f);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            
            Debug.Log(worldPosition);
            var positionToMove = new Vector3(worldPosition.x, 0f, worldPosition.z);
            rigidbody.MovePosition(positionToMove);
        }

    }
}
