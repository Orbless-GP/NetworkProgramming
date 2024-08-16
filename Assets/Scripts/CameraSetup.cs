using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using System;

public class CameraSetup : NetworkBehaviour
{
    public Camera playerCameraPrefab;
    private Camera playerCamera;
    public float yOffset = 2f; 

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            playerCamera = Instantiate(playerCameraPrefab);
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, -10);
            playerCamera.transform.LookAt(transform);

            playerCamera.transform.SetParent(transform);
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
