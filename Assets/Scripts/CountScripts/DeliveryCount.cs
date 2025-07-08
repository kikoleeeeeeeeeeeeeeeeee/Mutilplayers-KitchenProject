using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCount : BaseCounter
{
    public static DeliveryCount Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenOjbect())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //手中只有盘子 
                DliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                KitchenObject.DestoryKitchenObject(player.GetKitchenObject());
            }
        }
    }
}
  