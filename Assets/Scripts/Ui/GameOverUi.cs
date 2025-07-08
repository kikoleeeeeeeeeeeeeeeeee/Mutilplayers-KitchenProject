using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;



    private void Start()
    {
        GameManager.Instance.OnstateChanged += GameManager_OnstateChanged;
        Hide();
        playAgainButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown(); 
            Loader.Load(Loader.Scene.MainMenuSence);
        });
    }

    private void GameManager_OnstateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {

            Show();

            recipesDeliveredText.text = DliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();

        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
        playAgainButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
