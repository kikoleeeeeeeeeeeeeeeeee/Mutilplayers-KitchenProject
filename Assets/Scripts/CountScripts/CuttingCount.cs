using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCount : BaseCounter, Ihasprogress
{
    new public static void ResetStaticData()
    {
        onAnyCut = null;
    }
    public event EventHandler<Ihasprogress.onProgressCHANGedEventArgs> onProgressChanged;
    public event EventHandler OnCut;

    public static event EventHandler onAnyCut;

    [SerializeField] private CuttingrecipeSo[] cuttingRecipeSoArray;



    private int cuttingProgress;
    public override void Interact(Player player)
    {

        if (!HasKitchenOjbect())
        {
            if (player.HasKitchenOjbect())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {

                    KitchenObject kitchenObject = player.GetKitchenObject();

                    kitchenObject.SetkitchenObjectParent(this);
                    InteractLogicPlaceObjectOnCounterServerRpc();

                }
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenOjbect())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //手上有盘子
                    //PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                    }
                }
            }
            else
            {
                GetKitchenObject().SetkitchenObjectParent(player);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc()
    {
        InteractLogicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    private void InteractLogicPlaceObjectOnCounterClientRpc()
    {

        cuttingProgress = 0;

        onProgressChanged?.Invoke(this, new Ihasprogress.onProgressCHANGedEventArgs
        {
            progressNormalized = 0f
        });
    }


    public override void InteractAlternate(Player player)
    {
        if (HasKitchenOjbect() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectServerRpc();
            TestCuttingProgressDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        if (HasKitchenOjbect() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CutObjectClientRpc();
        }
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;

        OnCut?.Invoke(this, EventArgs.Empty);
        onAnyCut?.Invoke(this, EventArgs.Empty);

        CuttingrecipeSo cuttingrecipeSO = GetCuttingrecipeSoWithIpunt(GetKitchenObject().GetKitchenObjectSO());

        onProgressChanged?.Invoke(this, new Ihasprogress.onProgressCHANGedEventArgs
        {
            progressNormalized = (float)cuttingProgress / cuttingrecipeSO.cuttingProgressMax
        });


    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingProgressDoneServerRpc()
    {
        if (HasKitchenOjbect() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            CuttingrecipeSo cuttingrecipeSO = GetCuttingrecipeSoWithIpunt(GetKitchenObject().GetKitchenObjectSO());

            if (cuttingProgress >= cuttingrecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO ouputkitchenOjbecSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                KitchenObject.DestoryKitchenObject(GetKitchenObject());

                KitchenObject.SpawnKitchenObject(ouputkitchenOjbecSo, this);
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputkitchenObjectSo)
    {
        CuttingrecipeSo cuttingrecipeSO = GetCuttingrecipeSoWithIpunt(inputkitchenObjectSo);


        return cuttingrecipeSO != null;

    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputkitchenObjectSo)
    {
        CuttingrecipeSo cuttingrecipeSO = GetCuttingrecipeSoWithIpunt(inputkitchenObjectSo);
        if ((cuttingrecipeSO != null))
        {
            return cuttingrecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingrecipeSo GetCuttingrecipeSoWithIpunt(KitchenObjectSO inputkitchenObjectSo)
    {
        foreach (CuttingrecipeSo cuttingrecipeSo in cuttingRecipeSoArray)
        {
            if (cuttingrecipeSo.input == inputkitchenObjectSo)
            {
                return cuttingrecipeSo;
            }
        }
        return null;
    }
}
