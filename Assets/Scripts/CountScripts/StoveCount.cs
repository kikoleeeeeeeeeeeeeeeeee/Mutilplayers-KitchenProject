using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCount;

public class StoveCount : BaseCounter, Ihasprogress
{

    public event EventHandler<onStateChangedEventArgs> OnStateChanged;
    public event EventHandler<Ihasprogress.onProgressCHANGedEventArgs> onProgressChanged;
    public class onStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSo[] fryingRecipeSoArray;
    [SerializeField] private BurningRecipeSo[] burningRecipeSoArray;

    //被注释掉的是 用协程做成的计时器
    /*private void Start()
    {
        StartCoroutine(HandleFryTime());
    }
    private IEnumerator HandleFryTime()
    {
         yield return new WaitForSeconds(1f);
    }*/

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> FryTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSo fryingRecipeSo;
    private NetworkVariable<float> BurningTimer = new NetworkVariable<float>(0f);
    private BurningRecipeSo buringRecipeSo;

    public override void OnNetworkSpawn() {
        FryTimer.OnValueChanged += FryingTimer_OnValueChanged;
        BurningTimer.OnValueChanged += BurningTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }

    private void FryingTimer_OnValueChanged(float previousValue, float newValue)
    {
        float FryTimerMax = fryingRecipeSo != null ? fryingRecipeSo.fryingTimerMax : 1f;
        onProgressChanged?.Invoke(this, new Ihasprogress.onProgressCHANGedEventArgs
        {
            progressNormalized = FryTimer.Value / FryTimerMax
        });
    }

    private void BurningTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burningTimerMax = buringRecipeSo != null ? buringRecipeSo.burningTimerMax : 1f;
        onProgressChanged?.Invoke(this, new Ihasprogress.onProgressCHANGedEventArgs
        {
            progressNormalized = BurningTimer.Value / burningTimerMax
        });
    }

    private void State_OnValueChanged(State previousState, State newState)
    {
        OnStateChanged?.Invoke(this, new onStateChangedEventArgs
        {
            state = state.Value
        });

        if (state.Value == State.Burned || state.Value == State.Idle) {
            onProgressChanged?.Invoke(this, new Ihasprogress.onProgressCHANGedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenOjbect())
        {
            switch (state.Value)
            {

                case State.Idle:
                    break;
                case State.Frying:
                    FryTimer.Value += Time.deltaTime;

                    if (FryTimer.Value > fryingRecipeSo.fryingTimerMax)
                    {
                        KitchenObject.DestoryKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(fryingRecipeSo.output, this);


                        state.Value = State.Fried;

                        BurningTimer.Value = 0f;

                        SetBurningRecipeSoClientRpc(
                            KitchenGameMutilplayer.Instance.GetKitchenObjectSoIndex(GetKitchenObject().GetKitchenObjectSO())
                            );

                    }
                    break;
                case State.Fried:

                    BurningTimer.Value += Time.deltaTime;

                    if (BurningTimer.Value > buringRecipeSo.burningTimerMax)
                    {

                        KitchenObject.DestoryKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(buringRecipeSo.output, this);


                        state.Value = State.Burned;


                        OnStateChanged?.Invoke(this, new onStateChangedEventArgs
                        {
                            state = state.Value
                        });


                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenOjbect())
        {
            if (player.HasKitchenOjbect())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {

                    KitchenObject kitchenObject = player.GetKitchenObject();

                    player.GetKitchenObject().SetkitchenObjectParent(this);

                    InteractLogicPlaceObjectOnCounterServerRpc(
                            KitchenGameMutilplayer.Instance.GetKitchenObjectSoIndex(kitchenObject.GetKitchenObjectSO())
                        );

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

                        SetStateIdleServerRpc();

                    }
                }
            }
            else
            {
                GetKitchenObject().SetkitchenObjectParent(player);


                SetStateIdleServerRpc();
            }
        }
         
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        state.Value = State.Idle;

    }


    [ServerRpc(RequireOwnership = false)]    
    private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenobjectSoIndex)
    {
        FryTimer.Value = 0f;
        state.Value = State.Frying;


        SetFryingRecipeSoClientRpc(kitchenobjectSoIndex);
    }
    [ClientRpc]
    private void SetFryingRecipeSoClientRpc( int kitchenobjectSoIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMutilplayer.Instance.GetKitchenObjectSoFromIndex(kitchenobjectSoIndex);
        
        fryingRecipeSo = GetFryingrecipeSoWithIpunt(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSoClientRpc(int kitchenobjectSoIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMutilplayer.Instance.GetKitchenObjectSoFromIndex(kitchenobjectSoIndex);

        buringRecipeSo = GetBurningrecipeSoWithIpunt(kitchenObjectSO);
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputkitchenObjectSo)
    {
        FryingRecipeSo FryingRecipeSo = GetFryingrecipeSoWithIpunt(inputkitchenObjectSo);


        return FryingRecipeSo != null;

    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputkitchenObjectSo)
    {
        FryingRecipeSo FryRecipeSo = GetFryingrecipeSoWithIpunt(inputkitchenObjectSo);
        if ((FryRecipeSo != null))
        {
            return FryRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSo GetFryingrecipeSoWithIpunt(KitchenObjectSO inputkitchenObjectSo)
    {
        foreach (FryingRecipeSo fryRecipeSo in fryingRecipeSoArray)
        {
            if (fryRecipeSo.input == inputkitchenObjectSo)
            {
                return fryRecipeSo;
            }
        }
        return null;
    }

    private BurningRecipeSo GetBurningrecipeSoWithIpunt(KitchenObjectSO inpustkitchenObjectSo)
    {
        foreach (BurningRecipeSo burningRecipeSo in burningRecipeSoArray)
        {
            if (burningRecipeSo.input == inpustkitchenObjectSo)
            {
                return burningRecipeSo;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return state.Value ==State.Fried;
    }
}
