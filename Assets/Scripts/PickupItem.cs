using UnityEngine;
using Unity.Netcode;

public class PickupItem : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null && playerMovement.IsOwner)
            {
                playerMovement.CollectPickup();
                Destroy(gameObject);
            }
        }
    }
}
