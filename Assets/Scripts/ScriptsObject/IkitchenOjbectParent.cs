using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IkitchenOjbectParent
{
    public Transform GetKitchenObjectFollowTransform();


    public void SetKitchenOjbect(KitchenObject kitchenObject);


    public KitchenObject GetKitchenObject();


    public void ClearKitchenObject();


    public bool HasKitchenOjbect();
     
    public NetworkObject GetNetworkObject();
}
