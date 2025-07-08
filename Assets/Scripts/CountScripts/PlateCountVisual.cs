

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCountVisual : MonoBehaviour
{
    [SerializeField] private Transform countTopPoint;
    [SerializeField] private Transform PlateVisualPrefab;
    [SerializeField] private PlateCount plateCount;

    private List<GameObject> plateVisualGameObjectList;
    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }
    private void Start()
    {
        plateCount.OnplateSpawned += PlateCount_OnplateSpawned;
        plateCount.OnplateRemoved += PlateCount_OnplateRemoved;
    }

    private void PlateCount_OnplateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlateCount_OnplateSpawned(object sender, System.EventArgs e)
    {
      Transform PlateViusalTransFORM= Instantiate(PlateVisualPrefab, countTopPoint);

        float plateOffsev = .1f;
        PlateViusalTransFORM.localPosition = new Vector3(0, plateOffsev*plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(PlateViusalTransFORM.gameObject);
    }
}
     