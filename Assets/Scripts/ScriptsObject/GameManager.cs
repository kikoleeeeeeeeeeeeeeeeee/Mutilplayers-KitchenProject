using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using Mono.CSharp;
using Unity.VisualScripting;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance {  get; private set; }


    public event EventHandler OnstateChanged;
    public event EventHandler OnLocalGamePause;
    public event EventHandler OnLocalGameUnpause;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerUnGamePaused;
    public event EventHandler OnLocalPlayerReadyChange;
    private bool autoTestGamePausedState; 
    private enum State {
        WaitingToStart,
        CountDownToSTART,
        Gameplaying,
        Gameover,
    }

    [SerializeField] private Transform playerPrefab;

    private NetworkVariable<State> state  = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady; 
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayTimer = new NetworkVariable<float>(0f);
    private float gamePlayTimerMAX = 90f;
    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false); 
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPausedDictionary;
    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }
    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInterAction += gameinput_OnInterAction;
    }

    public override void OnNetworkSpawn()
    {
         state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong cliendId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(cliendId, true);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        autoTestGamePausedState = true;  
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerGamePaused?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnMultiplayerUnGamePaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnstateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void gameinput_OnInterAction(object sender, EventArgs e)
    {
        if (state.Value == State.WaitingToStart) { 
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChange?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();

        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default )
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                allClientsReady = false;
                break;
            }
        }
        if (allClientsReady)
        {
            state.Value = State.CountDownToSTART;  
        }
        Debug.Log("AllClientsReady: " + allClientsReady); 
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        PauseGame();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return; 
        }
        switch (state.Value)
        {
            case State.WaitingToStart:
                
                break;
            case State.CountDownToSTART:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f)
                {
                    state.Value = State.Gameplaying;
                    gamePlayTimer.Value = gamePlayTimerMAX;
                }
                break;
            case State.Gameplaying:
                gamePlayTimer.Value -= Time.deltaTime;
                if (gamePlayTimer.Value < 0f)
                {
                    state.Value = State.Gameover;
                    }
                break;
            case State.Gameover:
                
                break;
        }
        //Debug.Log(state);
    }

    private void LateUpdate()
    {
        if (autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePauseState();
        }
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.Gameplaying;
    }
    public bool IsCountdownToStartActive()
    {
        return state.Value ==State.CountDownToSTART;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return state.Value  ==State.Gameover;
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    } 

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public float GetPlayTimerNormal()
    {
        return 1-(gamePlayTimer.Value  / gamePlayTimerMAX);
    }
    public void PauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (isLocalGamePaused) {
            PauseGameServerRpc();

            OnLocalGamePause?.Invoke(this,EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();
            OnLocalGameUnpause?.Invoke(this,EventArgs.Empty);    
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams  = default)
    {   
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePauseState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams  = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePauseState();
    }

    private void TestGamePauseState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                // This player is paused
                isGamePaused.Value = true;
                return;
            }
        }

        // All players are unpaused
        isGamePaused.Value = false;
    }
}
