using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUi : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            KitchenObjectLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuSence);
        });

        createLobbyButton.onClick.AddListener(() =>
        {
            lobbyCreateUI.Show();
        });
        quickJoinButton.onClick.AddListener(() =>
        {
            KitchenObjectLobby.Instance.QuickJoin();
        });
        joinCodeButton.onClick.AddListener(() =>
        {
            KitchenObjectLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });

        lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerNameInputField.text = KitchenGameMutilplayer.Instance.GetPlayerName();
        playerNameInputField.onValueChanged.AddListener((string newText) =>
        {
            KitchenGameMutilplayer.Instance.SetPlayerName(newText);
        });
        KitchenObjectLobby.Instance.OnLobbyListChanged += KitchenObjectLobby_OnLobbyListChanged;
        updateLobbyList(new List<Lobby>());
    }
    private void KitchenObjectLobby_OnLobbyListChanged(object sender, KitchenObjectLobby.OnLobbyListChangedEventArgs e)
    {
        updateLobbyList(e.LobbyList);
    }

    private void updateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in lobbyContainer)
        {
            if(child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate,lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUi>().SetLobby(lobby);
        }
    }

    private void OnDestroy()
    {
        KitchenObjectLobby.Instance.OnLobbyListChanged -= KitchenObjectLobby_OnLobbyListChanged;
    }

}
