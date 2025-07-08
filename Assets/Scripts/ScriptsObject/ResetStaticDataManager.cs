using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        CuttingCount.ResetStaticData();
        BaseCounter.ResetStaticData();
        TranshCount.ResetStaticData();
        Player.ResetStaticData();
    }
}
