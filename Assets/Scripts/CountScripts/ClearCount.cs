using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCount : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (!HasKitchenOjbect())
        {
            //
            if (player.HasKitchenOjbect())
            {
                //玩家手上有东西
                player.GetKitchenObject().SetkitchenObjectParent(this);
            }
            else
            {
                //GetKitchenObject().SetkitchenObjectParent(player);
            }
        }
        else
        {
            if (player.HasKitchenOjbect())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    }
                }
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestoryKitchenObject(player.GetKitchenObject());                     
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetkitchenObjectParent(player);
            }
        }
    }
}
