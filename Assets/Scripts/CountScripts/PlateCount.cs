using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateCount : BaseCounter
{
    public event EventHandler OnplateSpawned;
    public event EventHandler OnplateRemoved;

    [SerializeField] private KitchenObjectSO plateKitChenOjbectSo;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax=4f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer >spawnPlateTimerMax ) {
            //KitchenObject.SpawnKitchenObject(plateKitChenOjbect, this);
            spawnPlateTimer = 0f;
            if (GameManager.Instance.IsGamePlaying() && plateSpawnAmount < plateSpawnAmountMax)
            {
                SpawnPlateServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }
    [ClientRpc]
    private  void SpawnPlateClientRpc()
    {
        plateSpawnAmount++;

        OnplateSpawned?.Invoke(this, EventArgs.Empty); 
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenOjbect())
        {
            if (plateSpawnAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitChenOjbectSo, player);

                InteractLogicServerRpc();
            }
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
        plateSpawnAmount--;

        OnplateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
