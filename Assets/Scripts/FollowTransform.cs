using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTransForm;
    public void SetTargetTransForm(Transform targetTransForm)
    {
        this.targetTransForm = targetTransForm;
    }
    private void LateUpdate()
    {
        if(targetTransForm == null)
        {
            return; 
        }
        transform.position = targetTransForm.position;
        transform.rotation = targetTransForm.rotation; 
    }
}
