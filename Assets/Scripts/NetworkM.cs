//using UnityEngine;
/*using Unity.Netcode;
using TMPro;

public class NetworkM : NetworkBehaviour
{
    public static NetworkM Instance { get; private set; }
    public static object Singleton { get; internal set; }

    public TMP_Text pickupCounterTextPrefab;
    private TMP_Text pickupCounterTextInstance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsClient)
        {
            SetupUI();
        }
    }

    private void SetupUI()
    {
        if (pickupCounterTextPrefab != null)
        {
            Canvas uiCanvas = FindObjectOfType<Canvas>();
            if (uiCanvas == null)a
            {
                return;
            }

            pickupCounterTextInstance = Instantiate(pickupCounterTextPrefab, uiCanvas.transform);
            pickupCounterTextInstance.gameObject.SetActive(true);

            UpdatePickupCount();
        }
    }

    public void UpdatePickupCount()
    {
        if (pickupCounterTextInstance != null)
        {
            Debug.Log("UpdatePickupCount - TotalPickups: " + PickupItem.TotalPickups.Value);
            // Update pickup count UI for this player
            pickupCounterTextInstance.text = "Pickups: " + PickupItem.TotalPickups.Value;
        }
    }

    [ClientRpc]
    public void UpdatePickupCountClientRpc()
    {
        Debug.Log("UpdatePickupCountClientRpc called");
        UpdatePickupCount();
    }
}
*/