using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] ScrollRect chatScrollRect;
    [SerializeField] TMP_InputField chatInput;

    private PlayerSettings playerSettings;
    private bool wasInputFieldFocused = false;

    void Awake()
    {
        ChatManager.Singleton = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            StartCoroutine(FindPlayerSettings());
        }
    }

    private IEnumerator FindPlayerSettings()
    {
        while (playerSettings == null)
        {
            foreach (var player in FindObjectsOfType<PlayerSettings>())
            {
                if (player.IsOwner)
                {
                    playerSettings = player;
                    break;
                }
            }

            if (playerSettings != null)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if (chatInput.isFocused)
        {
            if (!wasInputFieldFocused)
            {
                SetPlayerMovementEnabled(false);
                wasInputFieldFocused = true;
            }
        }
        else
        {
            if (wasInputFieldFocused)
            {
                SetPlayerMovementEnabled(true);
                wasInputFieldFocused = false;
            }
        }

        if (playerSettings != null && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrWhiteSpace(chatInput.text))
        {
            SendChatMessage(chatInput.text);
            chatInput.text = "";
        }
    }

    public void SendChatMessage(string _message)
    {
        if (string.IsNullOrWhiteSpace(_message) || playerSettings == null) return;

        string playerName = playerSettings.GetPlayerName();
        string formattedMessage = playerName + ": " + _message;
        SendChatMessageServerRpc(formattedMessage);
    }

    void AddMessage(string msg)
    {
        ChatMessage CM = Instantiate(chatMessagePrefab, chatScrollRect.content);
        CM.SetText(msg);

        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }

    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }

    private void SetPlayerMovementEnabled(bool enabled)
    {
        if (playerSettings != null)
        {
            var movementComponent = playerSettings.GetComponent<PlayerMovement>();
            if (movementComponent != null)
            {
                movementComponent.enabled = enabled;
            }
        }
    }
}
