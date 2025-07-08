 using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Netcode;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player player;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        animator.SetBool(IS_WALKING, player.IsWaking());
    }
}
