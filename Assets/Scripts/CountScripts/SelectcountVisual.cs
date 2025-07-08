using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectcountVisual : MonoBehaviour
{   
    [SerializeField] private BaseCounter  basecount;
    [SerializeField] private GameObject[] visualGameobjectArray;
    private void Start()
    {
        //
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnselectedCountChanged += Player_OnselectedCountChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnselectedCountChanged -= Player_OnselectedCountChanged;
            Player.LocalInstance.OnselectedCountChanged += Player_OnselectedCountChanged;
        }
    }

    private void Player_OnselectedCountChanged(object sender, Player.OnselectedCountChangedEventArgs e)
    {
        if(e.selectCount == basecount)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameobject in visualGameobjectArray)
        {
            visualGameobject.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (GameObject visualGameobject in visualGameobjectArray)
        {
            visualGameobject.SetActive(false);
        }
    }
}
