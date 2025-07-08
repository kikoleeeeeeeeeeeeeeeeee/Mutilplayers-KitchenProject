using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiPlayrUi : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.OnMultiplayerGamePaused += GameMananger_OnMultiplayerGamePaused;
        GameManager.Instance.OnMultiplayerUnGamePaused += GameManager_OnMultiplayerUnGamePaused;

        Hide();
    }

    private void GameManager_OnMultiplayerUnGamePaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameMananger_OnMultiplayerGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
