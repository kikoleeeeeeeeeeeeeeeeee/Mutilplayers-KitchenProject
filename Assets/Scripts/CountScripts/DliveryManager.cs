using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
public class DliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpwaned;
    public event EventHandler OnRecipeCompeleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipefailed;

    public static DliveryManager Instance { get; private set; }

    private List<RecipetSo> waitingrecipeSoList;
    [SerializeField] private RecipetListSo recipetListSo;

    private float spwanRecipeTime = 4f;
    private float spwanRecipeTimeMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmount;
    private void Awake()
    {
        Instance = this;

        waitingrecipeSoList = new List<RecipetSo>();

         

    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spwanRecipeTime -= Time.deltaTime;
        if (spwanRecipeTime <= 0f)
        {
            spwanRecipeTime = spwanRecipeTimeMax;

            if (GameManager.Instance.IsGamePlaying()&&waitingrecipeSoList.Count < waitingRecipeMax)
            {
                int waitingRecipeSoIndex = UnityEngine.Random.Range(0, recipetListSo.recipetSoList.Count);
                
                // Debug.Log(waitingRecipetSo.recipeName);
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSoIndex); 

            }
        }
    }
    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSoIndex)
    {

        RecipetSo waitingRecipetSo = recipetListSo.recipetSoList[waitingRecipeSoIndex];
        waitingrecipeSoList.Add(waitingRecipetSo);

        OnRecipeSpwaned?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingrecipeSoList.Count; ++i)
        {
            RecipetSo waitingRecipeSo = waitingrecipeSoList[i];

            if (waitingRecipeSo.kitchenObjectsoList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentMatchRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectsoList)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSo == recipeKitchenObjectSo)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        plateContentMatchRecipe = false;
                    }
                }
                if (plateContentMatchRecipe)
                {
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }
        DeliveryIncorrectRecipeServerRpc(); 
    }
    [ServerRpc(RequireOwnership = false)]
    private void DeliveryIncorrectRecipeServerRpc()
    {
        DeliveryIncorrectRecipeClientRpc(); // 修复方法名拼写
    }

    [ClientRpc]
    private void DeliveryIncorrectRecipeClientRpc() // 正确的方法名后缀 ClientRpc
    {
        OnRecipefailed?.Invoke(this, EventArgs.Empty);
    }


    [ServerRpc(RequireOwnership = false )]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSoListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSoListIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSoListIndex)
    {
        successfulRecipeAmount++;
        waitingrecipeSoList.RemoveAt(waitingRecipeSoListIndex);

        OnRecipeCompeleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }
    public  List<RecipetSo> GetWaitingRecipeSolist()
    {
        return waitingrecipeSoList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipeAmount;
    }   
}
