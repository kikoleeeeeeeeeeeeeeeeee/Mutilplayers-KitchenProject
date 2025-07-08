using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Mono.CSharp;
public class KitchenObject :NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    private IkitchenOjbectParent kitchenOjbectParent; 


    private FollowTransform followTransform; 
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }

    public void SetkitchenObjectParent(IkitchenOjbectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenOjbectParentNetWorkOjbectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenOjbectParentNetWorkOjbectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenOjbectParentNetWorkOjbectReference)
    {
        kitchenOjbectParentNetWorkOjbectReference.TryGet(out NetworkObject kitchenObjectParentNetworkOjbect);

        IkitchenOjbectParent kitchenOjbectParent = kitchenObjectParentNetworkOjbect.GetComponent<IkitchenOjbectParent>();
        if (this.kitchenOjbectParent != null)
        {
            this.kitchenOjbectParent.ClearKitchenObject();
        }
        this.kitchenOjbectParent = kitchenOjbectParent;

        if (kitchenOjbectParent.HasKitchenOjbect())
        {
            Debug.LogError("已经有一个kitchenoject");
        }

        kitchenOjbectParent.SetKitchenOjbect(this);

        followTransform.SetTargetTransForm(kitchenOjbectParent.GetKitchenObjectFollowTransform());
    }



    public IkitchenOjbectParent GetkitchenObjectParent( )
    {
        return kitchenOjbectParent;
    }
    public void DestorySelf()
    {
        Destroy(gameObject);
    }
    
    public void ClearKitchenObjectOnParent()
    {
        kitchenOjbectParent.ClearKitchenObject();
    }

    public bool TryGetPlate( out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject= null; 
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenOjbectParent kitchenOjbectParent)
    {
        KitchenGameMutilplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenOjbectParent);
    }

    public static void DestoryKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMutilplayer.Instance.DestoryKitchenObject(kitchenObject);
    }
}
