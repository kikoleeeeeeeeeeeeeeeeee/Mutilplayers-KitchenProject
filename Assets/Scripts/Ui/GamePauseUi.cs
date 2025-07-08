using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUi : MonoBehaviour
{

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    private void Start()
    {
        GameManager.Instance.OnLocalGamePause += GameManager_OnLocalGamePause; 
        GameManager.Instance.OnLocalGameUnpause += GameManager_OnLocalGameUnpause;

        Hide();
    }

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.PauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuSence);
        });
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionUi.Instance.Show(Show);
        });
    }
    private void GameManager_OnLocalGameUnpause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnLocalGamePause(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        optionsButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
