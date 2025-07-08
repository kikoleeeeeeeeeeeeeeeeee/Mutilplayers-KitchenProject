using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEngine;
public class Player : NetworkBehaviour, IkitchenOjbectParent
{

    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
    }
    public static Player LocalInstance { get; private set; }



    public event EventHandler OnpickSomething;

    public static Player instanceFiedld;
    public static Player GetInstanceField()
    {
        return instanceFiedld;
    }

    public static void SetInsatanceField(Player instanceField)
    {
        Player instanceFiedld = instanceField;
    }

    public event EventHandler<OnselectedCountChangedEventArgs> OnselectedCountChanged;
    public class OnselectedCountChangedEventArgs : EventArgs {
        public BaseCounter selectCount;
    }



    [SerializeField] private float MoveSpeed = 7f;
    [SerializeField] private LayerMask CountLaterMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private Transform kichenobjectHoldPoint;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private PlayerVisual playerVisual;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCount;
    private KitchenObject kitchenObject;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
        HandleInteraction();
    }
    private void Start()
    {
        GameInput.Instance.OnInterAction += Gameinput_OnInterAction;
        GameInput.Instance.OnInteractAlterion += Gameinput_OnInteractAlterion;

        PlayerData playerData = KitchenGameMutilplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(KitchenGameMutilplayer.Instance.GetPlayerColor(playerData.colorId));
    }


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        transform.position = spawnPositionList[KitchenGameMutilplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetWorkManager_OnClientDisconnectCallback;

        }

    }

    private void NetWorkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if(clientId == OwnerClientId && HasKitchenOjbect())
        {
            KitchenObject.DestoryKitchenObject(GetKitchenObject());
        }
    }

    private void Gameinput_OnInteractAlterion(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
            return;
        if (selectedCount != null)
        {
            selectedCount.InteractAlternate(this);
        }
    }

    private void Gameinput_OnInterAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
            return;
        if (selectedCount != null)
        {
            selectedCount.Interact(this);
        }
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactionDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycasthit, interactionDistance, CountLaterMask))
        {
            if (raycasthit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //判断是否有该组件 确定是否可以进行交互
                if (baseCounter != selectedCount)
                {
                    SetSelectedCount(baseCounter);
                }
            }
            else
            {
                SetSelectedCount(null);
            }
        }
        else
        {
            SetSelectedCount(null);
        }
    }
    public bool IsWaking()
    {
        return isWalking;
    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = MoveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeigh = 2f;
        bool CanMove = !Physics.BoxCast(transform.position, Vector3.one*playerRadius,  moveDir,Quaternion.identity, moveDistance, collisionsLayerMask);

        if (!CanMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            CanMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius,  moveDirX, Quaternion.identity, moveDistance, collisionsLayerMask);
            if (CanMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                CanMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius,  moveDirZ,Quaternion.identity, moveDistance, collisionsLayerMask);
                if (CanMove)
                {
                    moveDir = moveDirZ;   
                }
            }
        }

        if (CanMove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
        float RotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * RotateSpeed);
    }
    private void SetSelectedCount(BaseCounter selectedCount)
    {
        this.selectedCount = selectedCount;
        OnselectedCountChanged?.Invoke(this, new OnselectedCountChangedEventArgs
        {
            selectCount = selectedCount,
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kichenobjectHoldPoint;
    }
    public void SetKitchenOjbect(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnpickSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
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
