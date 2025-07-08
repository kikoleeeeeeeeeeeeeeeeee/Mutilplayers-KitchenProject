using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingOtherPlayersUi : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChange += GameManager_OnLocalPlayerReadyChange;
        GameManager.Instance.OnstateChanged += GameManager_OnstateChanged;
        Hide();
    }

    private void GameManager_OnstateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameManager_OnLocalPlayerReadyChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Show();
        }
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
