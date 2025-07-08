using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveFlashBarUi : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] private StoveCount StoveCount;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        StoveCount.onProgressChanged += StoveCount_onProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    } 

    private void StoveCount_onProgressChanged(object sender, Ihasprogress.onProgressCHANGedEventArgs e)
    {
        float burnShowProgress = .5f;
        bool show = StoveCount.IsFried() && e.progressNormalized >= burnShowProgress;

        animator.SetBool(IS_FLASHING,show);
    }
}
