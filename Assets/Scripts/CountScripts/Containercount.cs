using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Containercount : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public event EventHandler OnPlaterGrabbedObject;
    public override void Interact(Player player)
    {
        if (!player.HasKitchenOjbect())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        }

        InteractLogicServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();    
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlaterGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
  