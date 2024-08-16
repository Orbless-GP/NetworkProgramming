using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] public TMP_InputField nameInputField;

    private void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();

        hostButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
    }

    private void OnJoinButtonClicked()
    {
        NetworkManager.Singleton.StartClient();

        hostButton.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        nameInputField.gameObject.SetActive(false);
    }
}
