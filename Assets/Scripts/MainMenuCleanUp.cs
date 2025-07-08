using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanUp : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton!= null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
        if(KitchenGameMutilplayer.Instance!= null)
        {
            Destroy(KitchenGameMutilplayer.Instance.gameObject);
        }
        if (KitchenObjectLobby.Instance != null)
        {
            Destroy(KitchenGameMutilplayer.Instance.gameObject);
        }
    }
}
