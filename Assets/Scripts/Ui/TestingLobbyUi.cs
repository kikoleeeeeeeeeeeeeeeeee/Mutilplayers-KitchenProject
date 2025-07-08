using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUi : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;

    private void Awake()
    {
        createGameButton.onClick.AddListener(() =>
        {
            KitchenGameMutilplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScence);
        });
        joinGameButton.onClick.AddListener(() =>
        {
            KitchenGameMutilplayer.Instance.StartClient();
        });
    }

}
