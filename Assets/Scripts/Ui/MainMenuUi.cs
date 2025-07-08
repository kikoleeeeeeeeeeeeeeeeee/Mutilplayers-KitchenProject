using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button MultiplayerPlayButton;
    [SerializeField] private Button SinglePlayButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        MultiplayerPlayButton.onClick.AddListener(() =>
        {
            KitchenGameMutilplayer.playMultipalyer = true;
            Loader.Load(Loader.Scene.LobbyScence );
            //SceneManager.LoadScene(1);
            //通过索引切换场景  
            //在生成设置顶部可以看到场景的索引  
        });
        SinglePlayButton.onClick.AddListener(() =>
        {
            KitchenGameMutilplayer.playMultipalyer = false;
            Loader.Load(Loader.Scene.LobbyScence);
            
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
}
  