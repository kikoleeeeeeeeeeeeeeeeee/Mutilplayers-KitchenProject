using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuSence,
        GameSence,
        LoadingSence,
        LobbyScence,
        CharacterSelectScence,
    }
    private  static Scene targetSence;
    public static void Load(Scene targetScene)
    {
        Loader.targetSence = targetScene;

        SceneManager.LoadScene(Scene.LoadingSence.ToString());

    }

    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }    
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetSence.ToString());
    }
} 
