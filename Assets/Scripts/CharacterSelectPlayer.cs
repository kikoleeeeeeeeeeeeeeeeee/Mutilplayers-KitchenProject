using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{

    [SerializeField] private int playerIntex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshPro playerNameText;


    private void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = KitchenGameMutilplayer.Instance.GetPlayerDataFromPlayerIndex(playerIntex);
            KitchenObjectLobby.Instance.KickPlayer(playerData.playerId.ToString());
            KitchenGameMutilplayer.Instance.KickPlayer(playerData.clientId);
        });
    }
    private void Start() 
    {
        
        KitchenGameMutilplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMutilplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void KitchenGameMutilplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (KitchenGameMutilplayer.Instance.IsPlayerIndexConnected(playerIntex))
        {
            Show();

            PlayerData playerData = KitchenGameMutilplayer.Instance.GetPlayerDataFromPlayerIndex(playerIntex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));


            playerNameText.text = playerData.playerName.ToString();  

            playerVisual.SetPlayerColor(KitchenGameMutilplayer.Instance.GetPlayerColor(playerData.colorId));


        }
        else
        {
            Hide();
        }
        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

    }
    private void Show()
    {
        gameObject.SetActive(true);    
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMutilplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMutilplayer_OnPlayerDataNetworkListChanged;
    }

}
