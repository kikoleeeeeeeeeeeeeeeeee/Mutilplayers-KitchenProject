using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class LobbyMassageUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        KitchenGameMutilplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;
        KitchenObjectLobby.Instance.OnCreateLobbyStarted += KitchenObjectLobby_OnCreateLobbyStarted;
        KitchenObjectLobby.Instance.OnCreateLobbyFailed += KitchenObjectLobby_OnCreateLobbyFailed;
        KitchenObjectLobby.Instance.OnJoinStarted += KitchenObjectLobby_OnJoinStarted;
        KitchenObjectLobby.Instance.OnJoinFailed += KitchenObjectLobby_OnJoinFailed;
        KitchenObjectLobby.Instance.OnQuickJoinFailed += KitchenObjectLobby_OnQuickJoinFailed;

        Hide(); 
    }

    private void KitchenObjectLobby_OnQuickJoinFailed(object sender, System.EventArgs e)
    {
        ShowMassage("Could not find a Lobby to Quick Join!");

    }

    private void KitchenObjectLobby_OnJoinFailed(object sender, System.EventArgs e)
    {
        ShowMassage("Failed to create Lobby!");
    }

    private void KitchenObjectLobby_OnJoinStarted(object sender, System.EventArgs e)
    {
        ShowMassage("Joining Lobby...");
    }

    private void KitchenObjectLobby_OnCreateLobbyFailed(object sender, System.EventArgs e)
    {
        ShowMassage("Failed to create Lobby!");
    }

    private void KitchenObjectLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMassage("Create Lobby...");
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if(NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMassage("Failed to connect");
        }
        else
        {
            ShowMassage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMassage(string massage)
    {
        Show();
        messageText.text = massage;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        KitchenGameMutilplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
    }
}
