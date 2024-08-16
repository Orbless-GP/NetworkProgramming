using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    private Camera playerCamera;
    public float yOffset = 2f;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();

        if (!IsLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false); 
        }
    }

    private void LateUpdate()
    {
        if (IsLocalPlayer && playerCamera != null)
        {
            Vector3 newPosition = playerCamera.transform.position;
            newPosition.y = transform.position.y + yOffset;
            playerCamera.transform.position = newPosition;
        }
    }
}

