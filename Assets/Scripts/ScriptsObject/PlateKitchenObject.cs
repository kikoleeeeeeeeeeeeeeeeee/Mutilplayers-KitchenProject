using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<onInredientADDedEventArgs> OnIngreidentAdded;

    public class onInredientADDedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

    [SerializeField]private List<KitchenObjectSO>  validkitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;


    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validkitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }
        else
        {
            AddIngredfientServerRpc(
                KitchenGameMutilplayer.Instance.GetKitchenObjectSoIndex(kitchenObjectSO)
            );
            return true;
        } 
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredfientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredfientClientRpc(kitchenObjectSOIndex);
    }


    [ClientRpc]
    private void AddIngredfientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMutilplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSOIndex);
         kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngreidentAdded?.Invoke(this,new onInredientADDedEventArgs{
                KitchenObjectSO = kitchenObjectSO,
            });
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
