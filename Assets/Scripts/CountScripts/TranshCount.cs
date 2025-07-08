using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TranshCount : BaseCounter
{
    // Start is called before the first frame update

    public static event EventHandler OnAnyobjectTrasnhed;
     new public static void ResetStaticData()
    {
        OnAnyobjectTrasnhed = null;
    }
    public override void Interact(Player player)
    {    
        if (player.HasKitchenOjbect())
        {
            KitchenObject.DestoryKitchenObject(player.GetKitchenObject());


            InteractLogicServerRpc();

        }
    }
       
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyobjectTrasnhed?.Invoke(this, EventArgs.Empty);
    }
}
    