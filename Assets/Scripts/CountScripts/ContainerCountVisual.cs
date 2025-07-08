using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCountVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    private Animator animator;
    [SerializeField] private Containercount Containercount;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Containercount.OnPlaterGrabbedObject += Containercount_OnPlaterGrabbedObject;
    }

    private void Containercount_OnPlaterGrabbedObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
 