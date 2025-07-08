using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextGame : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnstateChanged += Delivery_OnstateChanged;
    }

    private void Delivery_OnstateChanged(object sender, System.EventArgs e)
    {
        if (DliveryManager.Instance.GetSuccessfulRecipesAmount() == 5) {

        }
    }
}
