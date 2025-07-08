using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMutilplayer : NetworkBehaviour
{
    public const int MAX_PLAYER_AMOUNT = 4;

    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";
    public static KitchenGameMutilplayer Instance { get; private set; }


    public static bool playMultipalyer;


    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler OnPlayerDataNetworkListChanged;

    [SerializeField] private kitchenObjectListSo kitchenObjectListSo;
    [SerializeField] private List<Color> playerColorList;

    private NetworkList<PlayerData> playerDataNetworkList;

    private string playerName;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate KitchenGameMultiplayer instance detected! Destroying the new one.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));

        playerDataNetworkList = new NetworkList<PlayerData>();
        playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;
    }


    private void Start()
    {
        if (!playMultipalyer)
        {
            StartHost();
            Loader.LoadNetwork(Loader.Scene.GameSence);
        }
        
    }
    public string GetPlayerName()
    {
       return playerName;
    }
        
    public void SetPlayerName(string playerName) { 
        this.playerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER,playerName);
    }

    private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientId == clientId)
            {
                //ц╩спа╢╫с
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientId = clientId,
            colorId = GetFirstUnusedColorId(),
        });
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);

    }




    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalrequest, NetworkManager.ConnectionApprovalResponse connectionApprovalresponse)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScence.ToString())
        {
            connectionApprovalresponse.Approved = false;
            connectionApprovalresponse.Reason = "Game has already started";
            return;
        }

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            connectionApprovalresponse.Approved = false;
            connectionApprovalresponse.Reason = "Game is Full";
        }

        connectionApprovalresponse.Approved = true;
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback1;
        NetworkManager.Singleton.StartClient();

    }

    private void NetworkManager_Client_OnClientConnectedCallback1(ulong client)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;

        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }
    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        throw new NotImplementedException();
    }

    private void NetworkManager_Client_OnClientDisconnectCallback(ulong obj)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenOjbectParent kitchenOjbectParent)
    {
        SpawnKitchenOjbectServerRpc(GetKitchenObjectSoIndex(kitchenObjectSO), kitchenOjbectParent.GetNetworkObject());
      }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenOjbectServerRpc(int KitchenObjectSoindex, NetworkObjectReference kitchenOjbectParentNetWorkOjbectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSoFromIndex(KitchenObjectSoindex);

        kitchenOjbectParentNetWorkOjbectReference.TryGet(out NetworkObject kitchenObjectParentNetworkOjbect);

        IkitchenOjbectParent kitchenOjbectParent = kitchenObjectParentNetworkOjbect.GetComponent<IkitchenOjbectParent>();

        if (kitchenOjbectParent.HasKitchenOjbect())
        {
            return; 
        }

        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        NetworkObject kitchenObjectNetworkObject = KitchenObjectTransform.GetComponent<NetworkObject>();

        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = KitchenObjectTransform.GetComponent<KitchenObject>();



        kitchenObject.SetkitchenObjectParent(kitchenOjbectParent);
    }

    public int GetKitchenObjectSoIndex(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectListSo.kitchenObjectSoList.IndexOf(kitchenObjectSO);
    }

    public KitchenObjectSO GetKitchenObjectSoFromIndex(int kitchenOjbectSoIndex)
    {
        return kitchenObjectListSo.kitchenObjectSoList[kitchenOjbectSoIndex];
    }

    public void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        DestoryKitchenObjectServerRpc(kitchenObject.NetworkObject);

    }


    [ServerRpc(RequireOwnership = false)]
    private void DestoryKitchenObjectServerRpc(NetworkObjectReference networkObjectNetworkObjectReference)
    {
        networkObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
       
        if(kitchenObjectNetworkObject == null)
        {
            return;
        }
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(networkObjectNetworkObjectReference);

        kitchenObject.DestorySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference networkObjectNetworkObjectReference)
    {
        networkObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectOnParent();
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }

    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientId == clientId)
            {
                return i;
            }
        }
        return -1;
    }
    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.clientId == clientId)
            {
                return playerData;
            }
        }
        return default;
    }
    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }
    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }

    public Color GetPlayerColor(int colorId)
    {
        return playerColorList[colorId];
    }
    public void ChangePlayerColor(int colorId)
    {
        ChangePlayerColorServerRpc(colorId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        if (!IsColorAvailable(colorId))
        {
            return;
        }
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.colorId = colorId;

        playerDataNetworkList[playerDataIndex] = playerData;
    }
    private bool IsColorAvailable(int colorId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.colorId == colorId)
            {
                return false;
            }
        }
        return true;
    }

    private int GetFirstUnusedColorId()
    {
        for (int i = 0; i < playerColorList.Count; i++)
        {
            if (IsColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }

    public void KickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        NetworkManager_Server_OnClientDisconnectCallback(clientId);
    }
}
