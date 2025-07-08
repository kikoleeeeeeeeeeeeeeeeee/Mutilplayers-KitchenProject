using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseCounter : NetworkBehaviour,IkitchenOjbectParent
{
    public static event EventHandler onAnyObjectPlaceHere;
    [SerializeField] private Transform countTopPoint;
    private KitchenObject kitchenObject;
    public static void ResetStaticData()
    {
        onAnyObjectPlaceHere = null;
    }
    public virtual void Interact(Player player)
    {
    }
    public virtual void InteractAlternate(Player player)
    {
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return countTopPoint;
    }
    public void SetKitchenOjbect(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null) { 
            onAnyObjectPlaceHere?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenOjbect()
    {
        return kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
