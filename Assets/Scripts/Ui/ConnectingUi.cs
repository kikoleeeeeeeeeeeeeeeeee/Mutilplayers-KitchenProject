using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUi : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        KitchenGameMutilplayer.Instance.OnTryingToJoinGame += KitchenGameMutilplayer_OnTryingToJoinGame;
        KitchenGameMutilplayer.Instance.OnFailedToJoinGame += KitchenGameMutilplayer_OnFailedToJoinGame;

        Hide();
    }

    private void KitchenGameMutilplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        Hide(); 
    }

    private void KitchenGameMutilplayer_OnTryingToJoinGame(object sender, System.EventArgs e)
    {
        Show();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true); 
    }
    private void OnDestroy()
    {
        KitchenGameMutilplayer.Instance.OnTryingToJoinGame -= KitchenGameMutilplayer_OnTryingToJoinGame;
        KitchenGameMutilplayer.Instance.OnFailedToJoinGame -= KitchenGameMutilplayer_OnFailedToJoinGame;
    }
}
